using System;
using System.Runtime.InteropServices;
using TheXDS.MCART.Attributes;
using TheXDS.Proteus.Models;

namespace TheXDS.Proteus.Plugins
{
    [Name("Tarjeta de regalo")]
    [Guid("89251779-7dc8-4561-a338-b7b98ca462a0")]
    public class GiftCardPaymentSource : RuleBasedPaymentSource<GiftCard, long>
    {
        /// <summary>
        /// Inicializa la clase <see cref="GiftCardPaymentSource"/>
        /// </summary>
        public GiftCardPaymentSource()
        {
            Failures.Add(((p, _) => p is null, "Tarjeta de regalo inválida."));
            Failures.Add(((p, _) => p.Void < DateTime.Now, "La tarjeta ha expirado."));
            Failures.Add(((p, _) => p.Used.HasValue, "La tarjeta ya ha sido redimida."));
        }

        protected override string Prompt => "Ingrese o escanee la tarjeta de regalo";

        protected override decimal? GetAmount(GiftCard? entity, Factura? f)
        {
            return entity?.Amount;
        }

        protected override void OnGeneratePayment(Factura f, GiftCard entity, PaymentInfo info)
        {
            entity.Used = DateTime.Now;
        }
    }
}
