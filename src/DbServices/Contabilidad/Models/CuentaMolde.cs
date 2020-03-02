using System.Collections.Generic;
using System.Linq;
using TheXDS.Proteus.Models.Base;

namespace TheXDS.Proteus.Models
{
    public class CuentaMolde : Nameable<int>
    {
        public virtual CuentaMolde? Parent { get; set; }
        public virtual List<CuentaMolde> Children { get; set; } = new List<CuentaMolde>();
        public virtual List<SubCuentaMolde> SubCuentas { get; set; } = new List<SubCuentaMolde>();
        public virtual Divisa? DefaultDivisa { get; set; }

        private static Cuenta ToCuenta(CuentaMolde p, int i)
        {
            var c = (Cuenta)p;
            c.Prefix = (short)(i + 1);
            return c;
        }
        private static SubCuenta ToCuenta(SubCuentaMolde p, int i)
        {
            var c = (SubCuenta)p;
            c.Prefix = (short)(i + 1);
            return c;
        }

        public static implicit operator Cuenta(CuentaMolde molde)
        {
            return new Cuenta
            {
                Name = molde.Name,
                Children = molde.Children.Select(ToCuenta).ToList(),
                DefaultDivisa = molde.DefaultDivisa,
                SubCuentas = molde.SubCuentas.Select(ToCuenta).ToList()
            };
        }
        public static implicit operator CuentaMolde(string name)
        {
            return new CuentaMolde
            {
                Name = name
            };
        }
    }
}