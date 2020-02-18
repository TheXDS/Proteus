using System.Collections.Generic;
using TheXDS.Proteus.Models.Base;

namespace TheXDS.Proteus.Models
{
    public class CtaXPagar : TimestampModel<long>
    {
        public virtual Proveedor Proveedor { get; set; }
        public virtual Empresa Empresa { get; set; }
        public string RefNum { get; set; }
        public decimal Total { get; set; }
        public bool Paid { get; set; }
        public virtual Partida CreationPartida { get; set; }
        public virtual List<CxpPayment> Payments { get; set; } = new List<CxpPayment>();
    }

    /*
     * Para consultar:
     * Se puede pagar una cuenta desde caja chica? (debería ser posible)
     * Es técnicamente posible pagar una cta.x.pag. con otros activos fijos?
     * Debe el sistema imprimir cheques? (probablemente sí, en matricial)
     * Se puede pagar una cta.x.pag. de forma parcial? o debe ser pagada por completo?
     */

    public class CxpPayment : TimestampModel<long>
    {
        public virtual CtaXPagar Parent { get; set; }
        public virtual Partida? PaymentPartida { get; set; }
        public decimal Monto { get; set; }

    }
}