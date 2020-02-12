using System.Collections.Generic;
using System.Linq;
using TheXDS.Proteus.Models.Base;

namespace TheXDS.Proteus.Models
{
    public class ProveedorXEmpresa : ModelBase<int>
    {
        public virtual Proveedor Proveedor { get; set; }
        public virtual Empresa Empresa { get; set; }
        public virtual SubCuenta DebitoCuenta { get; set; }
        public virtual SubCuenta CreditoCuenta { get; set; }
        public virtual SubCuenta GastoCuenta { get; set; }

        public override string ToString()
        {
            return $"{Proveedor} en {Empresa} (GPA: {DebitoCuenta?.FullCode}, CPP: {CreditoCuenta?.FullCode}, Gasto: {GastoCuenta?.FullCode})";
        }
    }

    public class Banco : Contact<int>
    {
        public virtual List<CuentaBanco> Cuentas { get; set; } = new List<CuentaBanco>();

        public IEnumerable<CuentaBanco> CuentasFor(Empresa e)
        {
            if (e is null) throw new System.ArgumentNullException(nameof(e));
            return Cuentas.Where(p => p.Cuenta.FindRoot().RootParent?.Id == e.Id);
        }
    }

    public class CuentaBanco : ModelBase<string>
    {
        public virtual SubCuenta Cuenta { get; set; }
    }


}