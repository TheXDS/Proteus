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
}