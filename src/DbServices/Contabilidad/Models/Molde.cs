using TheXDS.Proteus.Models.Base;

namespace TheXDS.Proteus.Models
{
    public class Molde : Nameable<int>
    {
        public virtual CuentaMolde Activo { get; set; }
        public virtual CuentaMolde Pasivo { get; set; }
        public virtual CuentaMolde Patrimonio { get; set; }
        public virtual CuentaMolde Ingresos { get; set; }
        public virtual CuentaMolde Costos { get; set; }
        public virtual CuentaMolde Gastos { get; set; }
    }
}
