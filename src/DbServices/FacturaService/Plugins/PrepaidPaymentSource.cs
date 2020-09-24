using System;
using System.Runtime.InteropServices;
using TheXDS.MCART.Attributes;
using TheXDS.Proteus.Models;

namespace TheXDS.Proteus.Plugins
{
    /// <summary>
    /// Método de pago intergado que permite cancelar el monto de una factura
    /// con fondos pre-pagados por el cliente.
    /// </summary>
    [Name("Créditos del cliente"), Description("Paga una factura con fondos pre-pagados del cliente.")]
    [Guid("dabcf30e-3c02-44d3-9937-ab339c897b88")]
    public class PrepaidPaymentSource : RuleBasedPaymentSource<Cliente, int>
    {
        public PrepaidPaymentSource()
        {
            Failures.Add(((c, _) => !c.CanPrepay, "El cliente no fue autorizado a precargar sus créditos."));
            Failures.Add((CheckCredit, "El cliente no tiene créditos suficientes."));

        }
        protected override string Prompt => "Ingrese o escanee la tarjeta de miembro del cliente";

        private static bool CheckCredit(Cliente c, PaymentInfo info)
        {
            return c.Prepaid < info.Amount;
        }

        protected override void OnGeneratePayment(Factura f, Cliente entity, PaymentInfo info)
        {
           entity.Prepaid -= info.Amount;
        }
    }
}
