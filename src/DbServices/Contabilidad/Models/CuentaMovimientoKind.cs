using TheXDS.MCART.Attributes;

namespace TheXDS.Proteus.Models
{
    public enum CuentaMovimientoKind : byte
    {
        Cheque,
        [Name("Transferencia bancaria")] Transferencia,
        [Name("Retiro / Depósito")] Efectivo
    }
}