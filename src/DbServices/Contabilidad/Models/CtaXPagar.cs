using TheXDS.Proteus.Models.Base;

namespace TheXDS.Proteus.Models
{
    public class CtaXPagar : TimestampModel<long>
    {
        public virtual Proveedor Proveedor { get; set; }
        public virtual Empresa Empresa { get; set; }
        public string RefNum { get; set; }
        public decimal Total { get; set; }
        public bool Paid { get; set; }
        public virtual Partida CreationPartida { get; set; }
        public virtual Partida? PaymentPartida { get; set; }
    }
}