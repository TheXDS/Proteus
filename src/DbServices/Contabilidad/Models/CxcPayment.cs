using TheXDS.Proteus.Models.Base;

namespace TheXDS.Proteus.Models
{
    public class CxcPayment : TimestampModel<long>
    {
        public virtual CtaXCobrar Parent { get; set; }
        public virtual Partida? PaymentPartida { get; set; }
        public decimal Monto { get; set; }
        public string RefNum { get; set; }
    }
}