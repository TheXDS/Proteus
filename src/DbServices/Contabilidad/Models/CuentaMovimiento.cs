using TheXDS.MCART.Types.Base;
using TheXDS.Proteus.Models.Base;

namespace TheXDS.Proteus.Models
{
    public class CuentaMovimiento : TimestampModel<long>, INameable
    {
        public string Name { get; set; }
        public string? Notes { get; set; }
        public virtual CuentaBanco Parent { get; set; }
        public decimal Monto { get; set; }
        public virtual Partida RefPartida { get; set; }

        public virtual Cliente? Source { get; set; }
        public virtual Proveedor? Beneficiario { get; set; }
        public virtual CuentaBanco? LocalTarget { get; set; }

        public TargetKind? OtherTarget { get; set; }
        public string? ExternalTarget { get; set; }

        public CuentaMovimientoKind Kind { get; set; }
    }
}