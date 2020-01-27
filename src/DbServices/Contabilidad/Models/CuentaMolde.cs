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

        public static implicit operator Cuenta(CuentaMolde molde)
        {
            return new Cuenta
            {
                Name = molde.Name,
                Children = molde.Children.Select(p => (Cuenta)p).ToList(),
                DefaultDivisa = molde.DefaultDivisa,
                SubCuentas = molde.SubCuentas.Select(p => (SubCuenta)p).ToList()
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

    public class SubCuentaMolde : Nameable<int>
    {
        public virtual CuentaMolde Parent { get; set; }

        public virtual Divisa? DefaultDivisa { get; set; }

        public static implicit operator SubCuenta(SubCuentaMolde molde)
        {
            return new SubCuenta
            {
                Name = molde.Name,
                Divisa = molde.DefaultDivisa,
                BalanceCache = 0m
            };
        }
        public static implicit operator SubCuentaMolde(string name)
        {
            return new SubCuentaMolde
            {
                Name = name
            };
        }

    }
}