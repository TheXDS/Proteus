using System.Collections.Generic;
using System.Linq;
using TheXDS.MCART.Types.Extensions;
using TheXDS.Proteus.Models.Base;

namespace TheXDS.Proteus.Models
{
    public class Cuenta : Nameable<int>
    {
        public virtual short Prefix { get; set; }
        public virtual List<Cuenta> Children { get; set; } = new List<Cuenta>();
        public virtual List<SubCuenta> SubCuentas { get; set; } = new List<SubCuenta>();
        public virtual Cuenta? Parent { get; set; }
        public virtual Divisa? DefaultDivisa { get; set; }
        public decimal BalanceCache { get; set; }

        public IEnumerable<ModelBase> JointTree => Children.Cast<ModelBase>().Concat(SubCuentas);

        public string FullCode => $"{Parent?.FullCode.OrNull("{0}.")}{Prefix}";
        public short FreeCuentaPrefix => (short)(Children.Any() ? Children.Max(p => p.Prefix) + 1 : 1);
        public short FreeSubCuentaPrefix => (short)(SubCuentas.Any() ? SubCuentas.Max(p => p.Prefix) + 1 : 1);
    }
}