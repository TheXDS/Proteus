using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TheXDS.MCART.Types.Base;
using TheXDS.MCART.Types.Extensions;
using TheXDS.Proteus.Models.Base;

namespace TheXDS.Proteus.Models
{
    /// <summary>
    /// Representa una sesión de caja.
    /// </summary>
    public class CajaOp : TimestampModel<int>
    {
        /// <summary>
        /// Estación de facturación donde se ha abierto la sesión de caja.
        /// </summary>
        public virtual Estacion Estacion { get; set; } = null!;

        /// <summary>
        /// Cajero que ha abierto la sesión de caja.
        /// </summary>
        public virtual Cajero Cajero { get; set; } = null!;

        /// <summary>
        /// Valor con el que la sesión de caja ha sido abierta.
        /// </summary>
        public decimal OpenBalance { get; set; }

        /// <summary>
        /// Marca de tiempo del cierre de la sesión de caja.
        /// </summary>
        /// <value>
        /// Un <see cref="DateTime"/> con la marca de tiempo del cierre de la
        /// sesión de caja, o <see langword="null"/> si la sesión de caja está
        /// abierta.
        /// </value>
        public DateTime? CloseTimestamp { get; set; }

        /// <summary>
        /// Balance de cierre de sesión de caja.
        /// </summary>
        public decimal? CloseBalance { get; set; }

        /// <summary>
        /// Colección de facturas creadas en esta sesión de caja.
        /// </summary>
        public virtual List<Factura> Facturas { get; set; } = new List<Factura>();

        public virtual List<CajaDrop> Drops { get; set; } = new List<CajaDrop>();
        public decimal TotalFacturas => Facturas.Sum(p => p.Total);
        public decimal TotalDrops => Drops.Sum(p => p.Amount);
        public decimal TotalEfectivo => Facturas.Sum(p => p.TotalPagadoEfectivo) - TotalDrops;

        public override string ToString()
        {
            return $"Sesión de caja{Estacion?.ToString().OrNull(" en {0}")}{Cajero?.ToString().OrNull(" por {0}")}";
        }
    }

    public class CajaDrop : TimestampModel<int>
    {
        public virtual CajaOp Parent { get; set; }
        public decimal Amount { get; set; }
        public string Concept { get; set; }
        public override string ToString()
        {
            return $"Salida de {Parent?.Estacion.Name ?? "caja"} por {Amount:C}";
        }
    }
}