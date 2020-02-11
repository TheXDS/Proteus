using TheXDS.Proteus.Models.Base;

namespace TheXDS.Proteus.Models
{
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