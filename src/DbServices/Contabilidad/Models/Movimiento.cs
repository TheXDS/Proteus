using TheXDS.Proteus.Models.Base;

namespace TheXDS.Proteus.Models
{
    public class Movimiento : ModelBase<long>
    {
        public virtual Partida Parent { get; set; }
        public virtual SubCuenta Cuenta { get; set; }
        public decimal RawValue { get; set; }
        public decimal? ExchangeRate { get; set; }
        public virtual CostCenter? CostCenter { get; set; }

        public static implicit operator decimal(Movimiento mov) => mov.RawValue;

        public override string ToString()
        {
            return $"{Cuenta?.FullCode} - {Cuenta?.Name}: {RawValue:C}";
        }

    }
}
