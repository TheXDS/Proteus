using TheXDS.Proteus.Models.Base;

namespace TheXDS.Proteus.Models
{
    public class CxpPayment : TimestampModel<long>
    {
        /*
         * Para consultar:
         * Se puede pagar una cuenta desde caja chica? (debería ser posible)
         * Es técnicamente posible pagar una cta.x.pag. con otros activos fijos?
         * Debe el sistema imprimir cheques? (probablemente sí, en matricial)
         * Se puede pagar una cta.x.pag. de forma parcial? o debe ser pagada por completo?
         */
        public virtual CtaXPagar Parent { get; set; }
        public virtual Partida? PaymentPartida { get; set; }
        public decimal Monto { get; set; }

    }
}