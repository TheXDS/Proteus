using System.Collections.Generic;
using TheXDS.Proteus.Models.Base;

namespace TheXDS.Proteus.Models
{
    public class Cuenta : Nameable<int>
    {
        public virtual List<Cuenta> Children { get; set; } = new List<Cuenta>();
        public virtual List<SubCuenta> SubCuentas { get; set; } = new List<SubCuenta>();
        public virtual Cuenta? Parent { get; set; }
    }
}
