using System.Collections.Generic;
using TheXDS.Proteus.Models.Base;

namespace TheXDS.Proteus.Models
{
    public class CtaXCobrar : TimestampModel<long>
    {
        public virtual Cliente Cliente { get; set; }
        public virtual Empresa Empresa { get; set; }
        public string RefNum { get; set; }
        public decimal Total { get; set; }
        public bool Paid { get; set; }
        public virtual Partida CreationPartida { get; set; }
        public virtual List<CxcPayment> Payments { get; set; } = new List<CxcPayment>();
    }
}