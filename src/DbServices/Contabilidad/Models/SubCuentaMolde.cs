using System;
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

    public class InventarioFijo : TimestampModel<string>, INameable
    {
        public int PiecesCount { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Describe el ciclo actual de depreciación. <see langword="null"/> para inventario fijo que no se deprecia.
        /// </summary>
        public int? DepreCycle { get; set; }

        /// <summary>
        /// Describe la última vez que una depreciación se ejecutó en este ítem.
        /// </summary>
        public DateTime? LastDepred { get; set; }
    }

    public enum RunPeriodicity : byte
    {
        Daily, Weekly, Monthly, Yearly
    }

    public class DepreMode : ModelBase<int>
    {
        public Guid DepreciadorGuid { get; set; }
        public int RunEach { get; set; } = 1;
        public RunPeriodicity Periodicity { get; set; } = RunPeriodicity.Yearly;
        public decimal ResidualValue { get; set; }
        public override string ToString()
        {
            return $"{Proteus.Service<ContabilidadService>()?.GetDepreciador(DepreciadorGuid)?.Name ?? DepreciadorGuid.ToString()} cada {Perioddesc()}";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string Perioddesc()
        {
            return Periodicity switch
            {
                RunPeriodicity.Daily => RunEach == 1 ? "día" : $"{RunEach} días",
                RunPeriodicity.Weekly => RunEach == 1 ? "semana" : $"{RunEach} semanas",
                RunPeriodicity.Monthly => RunEach == 1 ? "mes" : $"{RunEach} meses",
                RunPeriodicity.Yearly => RunEach == 1 ? "año" : $"{RunEach} años",
                _ => $"{RunEach} {Periodicity.NameOf().OrNull() ?? Periodicity.ToString()}"
            };
        }
    }
}