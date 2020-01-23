using System.Collections.Generic;
using System.Linq;
using TheXDS.Proteus.Models.Base;

namespace TheXDS.Proteus.Models
{
    public class CuentaMolde : Nameable<int>
    {
        public virtual CuentaMolde? Parent { get; set; }
        public virtual List<CuentaMolde> Children { get; set; } = new List<CuentaMolde>();
        public virtual Divisa? DefaultDivisa { get; set; }

        public static implicit operator Cuenta(CuentaMolde molde)
        {
            return new Cuenta
            {
                Name = molde.Name,
                Children = molde.Children.Select(p => (Cuenta)p).ToList(),
                DefaultDivisa = molde.DefaultDivisa
            };
        }
    }
}