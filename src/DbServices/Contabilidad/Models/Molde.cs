using TheXDS.Proteus.Models.Base;

namespace TheXDS.Proteus.Models
{
    public class Molde : Nameable<int>
    {
        public virtual CuentaMolde Activo { get; set; }
        public virtual CuentaMolde Pasivo { get; set; }
        public virtual CuentaMolde Patrimonio { get; set; }
    }
}
