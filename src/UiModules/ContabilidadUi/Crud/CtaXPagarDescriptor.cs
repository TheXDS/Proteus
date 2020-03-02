using System;
using TheXDS.Proteus.Api;
using TheXDS.Proteus.ContabilidadUi.Modules;
using TheXDS.Proteus.ContabilidadUi.ViewModels;
using TheXDS.Proteus.Crud.Base;
using TheXDS.Proteus.Models;
using TheXDS.Proteus.Models.Base;

namespace TheXDS.Proteus.ContabilidadUi.Crud
{
    /// <summary>
    /// Describe las propiedades Crud para el modelo
    /// <see cref="CtaXPagar"/>.
    /// </summary>
    public class CtaXPagarDescriptor : CrudDescriptor<CtaXPagar, CtaXPagarViewModel>
    {
        /// <summary>
        /// Describe las propiedades Crud para el modelo
        /// <see cref="CtaXPagar"/>.
        /// </summary>
        protected override void DescribeModel()
        {
            OnModuleMenu(Annotations.InteractionType.AdminTool);
            FriendlyName("Cuenta por pagar");
            ObjectProperty(p => p.Proveedor).Selectable().Required().Important();
            Property(p => p.Timestamp).Important("Fecha de emisión").Default(DateTime.Today);
            VmObjectProperty(p => p.Kind).Selectable().Required().Label("Tipo de documento");
            Property(p => p.RefNum).Required().Important("# de ref. de factura/cuenta");
            NumericProperty(p => p.Total).Positive().Important("Total a pagar");
            Property(p => p.Paid).Important("Pagada").Hidden();

            VmBeforeSave(BuildPartida);
            AfterSave(async () => await ContabilidadModule.ModuleStatus.InitViewModel());
        }

        private void BuildPartida(CtaXPagarViewModel arg1, ModelBase arg2)
        {            
            var cta = arg1.Entity;
            if (cta.CreationPartida is { }) return;
            cta.Empresa = ContabilidadModule.ModuleStatus.ActiveEmpresa!;
            var current = Proteus.Service<ContabilidadService>()!.First<ProveedorXEmpresa>(p => p.Empresa.Id == cta.Empresa!.Id);

            cta.CreationPartida = new Partida
            {
                Description = $"{arg1.Kind.Name} por pagar de proveedor {cta.Proveedor} por {cta.Total:C}, número de referencia {cta.RefNum}",
                Entidad = ContabilidadModule.ModuleStatus.ActiveEntidad,
                Documentos =
                {
                    new DocumentRef
                    {
                        Kind = arg1.Kind,
                        DocReference = cta.RefNum
                    }
                },
                Timestamp = DateTime.Now,
                Parent = ContabilidadModule.ModuleStatus.ActivePeriodo!,
                Movimientos =
                {
                    new Movimiento
                    {
                        RawValue = cta.Total,
                        Cuenta = current.GastoCuenta
                    },
                    new Movimiento
                    {
                        RawValue = -cta.Total,
                        Cuenta = current.CreditoCuenta
                    }
                }
            };
        }
    }

    /// <summary>
    /// Describe las propiedades Crud para el modelo
    /// <see cref="CxcPayment"/>.
    /// </summary>
    public class CxcPaymentCrudDescriptor : CrudDescriptor<CxcPayment, CxcPaymentViewModel>
    {
        /// <summary>
        /// Describe las propiedades Crud para el modelo
        /// <see cref="CxcPayment"/>.
        /// </summary>
        protected override void DescribeModel()
        {
            DateProperty(p => p.Timestamp).WithTime().Required().Important("Fecha de pago");
            NumericProperty(p => p.Monto).Positive().Required().Important();
            TextProperty(p => p.RefNum).NotEmpty().Required().Important("Número de referencia");
            VmBeforeSave(BuildPartida);
        }

        private void BuildPartida(CxcPaymentViewModel arg1, ModelBase parent)
        {
            if (!(parent is CtaXCobrar cuenta)) throw new InvalidOperationException("NOP! Can't do it...");
            var cta = arg1.Entity;
            if (cta.PaymentPartida is { }) return;
            var current = Proteus.Service<ContabilidadService>()!.First<ClienteXEmpresa>(p => p.Empresa.Id == cuenta.Empresa!.Id);
            cta.Parent = cuenta;
            cta.PaymentPartida = new Partida
            {
                Description = $"Abono a cuenta por cobrar de {cuenta.Cliente} por {cta.Monto}",
                Entidad = ContabilidadModule.ModuleStatus.ActiveEntidad,
                Documentos =
                {
                    new DocumentRef
                    {
                        Kind = arg1.Kind,
                        DocReference = cta.RefNum
                    }
                },
                Timestamp = DateTime.Now,
                Parent = ContabilidadModule.ModuleStatus.ActivePeriodo!,
                Movimientos =
                {
                    new Movimiento
                    {
                        RawValue = cta.Monto,
                        Cuenta = current.IngresoCuenta
                    },
                    new Movimiento
                    {
                        RawValue = -cta.Monto,
                        Cuenta = current.DebitoCuenta
                    }

                }
            };
        }
    }
}