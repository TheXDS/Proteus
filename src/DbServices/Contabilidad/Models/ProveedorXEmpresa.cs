using System.Collections.Generic;
using System.Linq;
using TheXDS.MCART.Attributes;
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

    public enum CuentaBancoKind : byte
    {
        Ahorros,
        Cheques
    }
    public class CuentaBanco : ModelBase<string>
    {
        public virtual CuentaBancoKind Kind { get; set; }
        public virtual SubCuenta Cuenta { get; set; }
        public virtual List<CuentaMovimiento> Movimientos { get; set; } = new List<CuentaMovimiento>();
    }

    /*
    public enum CuentaMovimientoKind : byte
    {
        Cheque,
        Transferencia,
        Retiro,
        [Name("Depósito")] Deposito
    }

    public class ChequeIn : CuentaMovimientoIn
    {
        public string Concepto { get; set; }
    }

    public class ChequeOut : CuentaMovimientoOut
    {
        public string Concepto { get; set; }
    }


    public class TransferenciaIn : CuentaMovimientoIn
    {
    }

    public class TransferenciaOut : CuentaMovimientoOut
    {
    }

    public class Retiro : CuentaMovimiento
    {
    }

    public abstract class CuentaMovimientoIn : CuentaMovimiento
    {
        public virtual Cliente Source { get; set; }
    }
    public abstract class CuentaMovimientoOut : CuentaMovimiento
    {
        public virtual Proveedor Beneficiario { get; set; }
    }
    public abstract class CuentaMovimiento : TimestampModel<long>
    {
        public string NumRef { get; set; }
        public virtual CuentaBanco Parent { get; set; }
        public decimal Monto { get; set; }
        public virtual Partida RefPartida { get; set; }
    }
    */
    public enum CuentaMovimientoKind : byte
    {
        Cheque,
        Transferencia,
        Efectivo
    }

    public enum TargetKind : byte
    {
        [Name("Banco externo")]ExternalBanco,
    }

    public class CuentaMovimiento : TimestampModel<long>
    {
        public string NumRef { get; set; }
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