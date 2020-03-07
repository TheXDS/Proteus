using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TheXDS.MCART.Types.Base;
using TheXDS.MCART.Types.Extensions;
using TheXDS.Proteus.Api;
using TheXDS.Proteus.Models.Base;

namespace TheXDS.Proteus.Models
{
    public class SubCuentaMolde : Nameable<int>
    {
        public virtual CuentaMolde Parent { get; set; }

        public virtual Divisa? DefaultDivisa { get; set; }

        public static implicit operator SubCuenta(SubCuentaMolde molde)
        {
            return new SubCuenta
            {
                Name = molde.Name,
                Divisa = molde.DefaultDivisa,
                BalanceCache = 0m
            };
        }

        public static implicit operator SubCuentaMolde(string name)
        {
            return new SubCuentaMolde
            {
                Name = name
            };
        }
    }
    public class InventarioKind : Nameable<int>
    {
        /// <summary>
        /// Describe el modo actual de depreciación. <see langword="null"/>
        /// para inventario fijo que no se deprecia.
        /// </summary>
        public virtual DepreMode? Depreciacion { get; set; }

        /// <summary>
        /// Obtiene la unidad en la que se mide la vida útil de un inventario fijo.
        /// </summary>
        public TimeUnit LifeUnit { get; set; }

        /// <summary>
        /// Obtiene el valor de vida útil del inventario fijo.
        /// </summary>
        public int LifeValue { get; set; }

    }

    public class InventarioFijo : TimestampModel<string>, INameable
    {
        public virtual InventarioKind Kind { get; set; }
        public string Name { get; set; }
        public int PiecesCount { get; set; }
        public decimal ValorInicial { get; set; }
        public virtual List<Depreciacion> Depreciaciones { get; set; } = new List<Depreciacion>();
        public decimal ValorResidual
        {
            get
            {
                return Kind.Depreciacion is { PercentValue: float p }
                    ? ValorInicial - (ValorInicial * (decimal)p)
                    : Kind.Depreciacion?.StaticValue ?? ValorInicial;
            }
        }

    }

    public class Depreciacion: TimestampModel<long>
    {
        public virtual InventarioFijo InvFijo { get; set; }
        public decimal Amount { get; set; }
    }


    public enum TimeUnit : byte
    {
        Days, Weeks, Months, Years
    }

    public class DepreMode : Valuable<int>
    {
        public Guid DepreciadorGuid { get; set; }
        public int RunEach { get; set; } = 1;
        public TimeUnit Periodicity { get; set; } = TimeUnit.Years;
        public override string ToString()
        {
            return $"{Proteus.Service<ContabilidadService>()?.GetDepreciador(DepreciadorGuid)?.Name ?? DepreciadorGuid.ToString()} cada {Perioddesc()}";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string Perioddesc()
        {
            return Periodicity switch
            {
                TimeUnit.Days => RunEach == 1 ? "día" : $"{RunEach} días",
                TimeUnit.Weeks => RunEach == 1 ? "semana" : $"{RunEach} semanas",
                TimeUnit.Months => RunEach == 1 ? "mes" : $"{RunEach} meses",
                TimeUnit.Years => RunEach == 1 ? "año" : $"{RunEach} años",
                _ => $"{RunEach} {Periodicity.NameOf().OrNull() ?? Periodicity.ToString()}"
            };
        }
    }
}