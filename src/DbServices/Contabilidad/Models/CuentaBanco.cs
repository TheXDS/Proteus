using System.Collections.Generic;
using TheXDS.Proteus.Models.Base;

namespace TheXDS.Proteus.Models
{
    public class CuentaBanco : ModelBase<string>
    {
        public virtual CuentaBancoKind Kind { get; set; }
        public virtual SubCuenta Cuenta { get; set; }
        public virtual List<CuentaMovimiento> Movimientos { get; set; } = new List<CuentaMovimiento>();

        public virtual Banco Parent { get; set; }
        public override string ToString()
        {
            return $"Cuenta de {Kind} en {Parent} #{Id}";
        }
    }
}