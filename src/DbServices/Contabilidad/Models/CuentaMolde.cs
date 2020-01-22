using System.Collections.Generic;
using TheXDS.Proteus.Models.Base;

namespace TheXDS.Proteus.Models
{
    public class CuentaMolde : Nameable<int>
    {
        public virtual CuentaMolde? Parent { get; set; }
        public virtual List<CuentaMolde> Children { get; set; } = new List<CuentaMolde>();
        public virtual Divisa? DefaultDivisa { get; set; }
    }
}
