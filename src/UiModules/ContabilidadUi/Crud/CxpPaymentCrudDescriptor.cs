using System;
using TheXDS.Proteus.Api;
using TheXDS.Proteus.ContabilidadUi.Modules;
using TheXDS.Proteus.ContabilidadUi.ViewModels;
using TheXDS.Proteus.Crud.Base;
using TheXDS.Proteus.Models;
using TheXDS.Proteus.Models.Base;

namespace TheXDS.Proteus.ContabilidadUi.Crud
{
    public class CxpPaymentCrudDescriptor : CrudDescriptor<CxpPayment, CxpPaymentViewModel>
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

        private void BuildPartida(CxpPaymentViewModel arg1, ModelBase parent)
        {
            if (!(parent is CtaXPagar cuenta)) throw new InvalidOperationException("NOP! Can't do it...");
            var cta = arg1.Entity;
            if (cta.PaymentPartida is { }) return;
            var current = Proteus.Service<ContabilidadService>()!.First<ProveedorXEmpresa>(p => p.Empresa.Id == cuenta.Empresa!.Id);
            cta.Parent = cuenta;
            cta.PaymentPartida = new Partida
            {
                Description = $"Abono a cuenta por pagar #{cuenta.RefNum} de {cuenta.Proveedor}",
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
                        RawValue = -cta.Monto,
                        Cuenta = current.GastoCuenta
                    },
                    new Movimiento
                    {
                        RawValue = cta.Monto,
                        Cuenta = current.CreditoCuenta
                    }
                }
            };
        }
    }
}