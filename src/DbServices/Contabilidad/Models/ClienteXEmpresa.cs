using TheXDS.Proteus.Models.Base;

namespace TheXDS.Proteus.Models
{
    public class ClienteXEmpresa : ModelBase<int>
    {
        public virtual Cliente Cliente { get; set; }
        public virtual Empresa Empresa { get; set; }
        public virtual SubCuenta DebitoCuenta { get; set; }
        public virtual SubCuenta CreditoCuenta { get; set; }
        public virtual SubCuenta IngresoCuenta { get; set; }

        public override string ToString()
        {
            return $"{Empresa} (CPC: {DebitoCuenta?.FullCode}, CPA: {CreditoCuenta?.FullCode}, Ingresos: {IngresoCuenta?.FullCode})";
        }
    }
}