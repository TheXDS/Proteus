using System.Collections.Generic;
using TheXDS.Proteus.Models.Base;

namespace TheXDS.Proteus.Models
{
    public class SubCuenta : Nameable<int>
    {
        public virtual Cuenta Parent { get; set; }
        public virtual List<Movimiento> Movimientos { get; set; } = new List<Movimiento>();
        public virtual Divisa? DefaultDivisa { get; set; }
        public decimal BalanceCache { get; set; }
    }
}