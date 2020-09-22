using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using TheXDS.Proteus.Api;
using TheXDS.Proteus.Models;

namespace TheXDS.Proteus.Plugins
{
    [Guid("89251779-7dc8-4561-a338-b7b98ca462a0")]
    public class GiftCardPaymentSource : PaymentSource
    {
        public override Task<Payment?> TryPayment(Factura fact, decimal amount)
        {
            long gcid = 0;
            if (!Proteus.InputTarget?.Get("Ingrese o escanee la tarjeta de regalo", ref gcid) ?? false)
                return Task.FromResult<Payment?>(null);
            var gc = Proteus.Service<FacturaService>()!.Get<GiftCard, long>(gcid);
            if (gc is null) return Task.FromResult<Payment?>(null);
            gc.Void = DateTime.Now;
            return base.TryPayment(fact, gc.Amount);
        }
    }
}
