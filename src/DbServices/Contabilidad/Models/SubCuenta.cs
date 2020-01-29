using System.Collections.Generic;
using TheXDS.Proteus.Models.Base;
using TheXDS.MCART.Types.Extensions;
using TheXDS.Proteus.Component;
using TheXDS.Proteus.Reporting;
using System;

namespace TheXDS.Proteus.Models
{
    public class SubCuenta : Nameable<int>
    {
        public virtual short Prefix { get; set; }
        public virtual Cuenta Parent { get; set; }
        public virtual List<Movimiento> Movimientos { get; set; } = new List<Movimiento>();
        public virtual Divisa? Divisa { get; set; }
        public decimal BalanceCache { get; set; }

        public string FullCode => $"{Parent?.FullCode.OrNull("{0}-")}{Prefix}";
    }

}