using System;
using System.Runtime.InteropServices;
using TheXDS.MCART.Attributes;
using TheXDS.Proteus.Api;
using TheXDS.Proteus.Models;

namespace TheXDS.Proteus.Plugins
{
    /// <summary>
    /// Método de pago que admite dejar un saldo pendiente a ser cancelado al
    /// crédito.
    /// </summary>
    [Name("Al crédito"), Description("Paga una factura por medio del crédito disponible del cliente.")]
    [Guid("4b919e69-0108-45d7-95eb-9b3c60c5ae93")]
    public class CreditPaymentSource : RuleBasedPaymentSource<Cliente, int>
    {
        public override PaymentInfo? Automatic(Factura? f)
        {
           return PaymentInfo.Manual(f?.Cliente?.Id.ToString());
        }

        public CreditPaymentSource()
        {
            Failures.Add(((c, _) => c is null, "El cliente no existe."));
            Failures.Add(((c, _) => !c!.CanCredit, "No se puede otorgar crédito al cliente."));
            Failures.Add((CheckCredit, "El cliente excede su límite permitido de crédito."));
        }

        protected override string Prompt => "Ingrese o escanee la tarjeta de miembro del cliente";

        private static bool CheckCredit(Cliente? c, PaymentInfo info)
        {
            return c!.AvailableCredit is { } v && info.Amount > v;
        }
        protected override Cliente? GetEntity(Factura fact, PaymentInfo? info)
        {
            return fact.Cliente ?? base.GetEntity(fact, info);
        }
        protected override void OnGeneratePayment(Factura f, Cliente entity, PaymentInfo info)
        {
            FacturaXCobrar fxc = new FacturaXCobrar
            {
                Cliente = entity,
                Parent = f,
                Total = info.Amount,
            };
            Proteus.Service<FacturaService>()!.Add(fxc);
            Proteus.Service<FacturaService>()!.SaveAsync().GetAwaiter().GetResult();
            info.Tag = fxc.StringId;
        }
    }
}
