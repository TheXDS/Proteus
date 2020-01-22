using TheXDS.Proteus.Models.Base;

namespace TheXDS.Proteus.Models
{
    public class Movimiento : ModelBase<long>
    {
        public virtual Partida Parent { get; set; }
        public virtual SubCuenta Cuenta { get; set; }
        public decimal RawValue { get; set; }
        public decimal? ExchangeRate { get; set; }
    }
}
