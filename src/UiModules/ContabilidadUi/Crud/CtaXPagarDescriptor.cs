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
            //ObjectProperty(p => p.Empresa).Selectable().Required().Important().Default(ContabilidadModule.ModuleStatus?.ActiveEmpresa);
            ObjectProperty(p => p.Proveedor).Selectable().Required().Important();
            Property(p => p.Timestamp).Important("Fecha de emisión").Default(DateTime.Today);
            VmObjectProperty(p => p.Kind).Selectable().Required().Label("Tipo de documento");
            Property(p => p.RefNum).Required().Important("# de ref. de factura/cuenta");
            NumericProperty(p => p.Total).Positive().Important("Total a pagar");
            Property(p => p.Paid).Important("Pagada").Hidden();
            //ObjectProperty(p => p.CreationPartida).Creatable().Required().Important("Partida de costo");
            //ObjectProperty(p => p.PaymentPartida).Creatable().Nullable().Important("Partida de pago");

            VmBeforeSave(BuildPartida);
            AfterSave(async () => await ContabilidadModule.ModuleStatus.InitViewModel());
        }

        private void BuildPartida(CtaXPagarViewModel arg1, ModelBase arg2)
        {            
            var cta = arg1.Entity;
            if (cta.CreationPartida is { }) return;
            cta.Empresa = ContabilidadModule.ModuleStatus.ActiveEmpresa!;
            var current = Proteus.Service<ContabilidadService>()!.First<ProveedorXEmpresa>(p => p.Empresa.Id == ContabilidadModule.ModuleStatus.ActiveEmpresa!.Id);

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
}