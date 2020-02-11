using System.Collections.Generic;
using TheXDS.MCART.Types.Extensions;
using TheXDS.Proteus.Models.Base;

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

    public class Proveedor : Addressable<int>
    {
        public string Rtn { get; set; }
        public int DaysDue { get; set; } = 30;
        public virtual List<ProveedorXEmpresa> Empresas { get; set; } = new List<ProveedorXEmpresa>();
    }

    public class ProveedorXEmpresa : ModelBase<int>
    {
        public virtual Proveedor Proveedor { get; set; }
        public virtual Empresa Empresa { get; set; }
        public virtual SubCuenta DebitoCuenta { get; set; }
        public virtual SubCuenta CreditoCuenta { get; set; }

    }

    public class CtaXPagar : TimestampModel<long>
    {
        public virtual Proveedor Proveedor { get; set; }
        public string RefNum { get; set; }
        public decimal Total { get; set; }
        public bool Paid { get; set; }
        public virtual Partida CreationPartida { get; set; }
        public virtual Partida? PaymentPartida { get; set; }
    }
}