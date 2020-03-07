using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using TheXDS.MCART;
using TheXDS.MCART.Types.Extensions;
using TheXDS.Proteus.Component;
using TheXDS.Proteus.Context;
using TheXDS.Proteus.Models;
using TheXDS.Proteus.Plugins;

namespace TheXDS.Proteus.Api
{
    public class ContabilidadService : Service<ContabilidadContext>
    {
        public IReadOnlyDictionary<Guid, IDepreciador> Depreciadores { get; private set; }

        protected override async Task AfterInitialization(IStatusReporter? reporter)
        {
            reporter?.UpdateStatus("Cargando herramientas de depreciación");

            Depreciadores = Objects.FindAllObjects<IDepreciador>().ToDictionary(p => p.Guid);
        }
        public AccessControlList? GetList()
        {
            return GetUser<AccessControlList>(this);
        }
        public IDepreciador? GetDepreciador(Guid guid)
        {
            return Depreciadores.ContainsKey(guid) ? Depreciadores[guid] : null;
        }
        public async Task<DetailedResult> NewPeriod(Empresa obj)
        {
            var epoch = DateTime.Now;
            if (!obj.Periodos.Any())
            {
                obj.Periodos.Add(new Periodo { Timestamp = epoch.Date });
                return DetailedResult.Ok;
            }

            var lastPeriodo = obj.Periodos.Last();
            lastPeriodo.Void = epoch;

            var newPeriodo = new Periodo
            {
                Timestamp = epoch
            };


            var ent = await All<Partida>().Where(p => p.Parent.Id == lastPeriodo.Id).Select(p=>p.Entidad).Distinct().ToListAsync();

            foreach (var e in ent)
            {
                var movs = await All<Movimiento>()
                    .Where(p=>p.Parent.Parent.Id == lastPeriodo.Id)
                    .Select(p => new { p.Cuenta, p.RawValue, p.CostCenter })
                    .GroupBy(p => new { p.Cuenta, p.CostCenter }, p => p.RawValue)                    
                    .ToListAsync();

                var partida = new Partida
                {
                    Description = $"Apertura de nuevo periodo contable {DateTime.Today:MMMM yyyy}, saldos iniciales{e?.Name.OrNull(" de {0}")}",
                    Timestamp = epoch
                };
                foreach (var j in movs)
                {
                    partida.Movimientos.Add(new Movimiento
                    {
                        Cuenta = j.Key.Cuenta,
                        CostCenter = j.Key.CostCenter,
                        RawValue = j.Sum()
                    });
                }
                newPeriodo.Partidas.Add(partida);
            }
            lastPeriodo.Parent.Periodos.Add(newPeriodo);
            return await InternalSaveAsync();
        }

        public static IQueryable<CtaXPagar> Pendientes
        {
            get
            {
                return Proteus.Service<ContabilidadService>()!.All<CtaXPagar>()
                    .Where(p => p.Paid == false);
            }
        }

        public static IEnumerable<CtaXPagar> Vencidas
        {
            get
            {
                return Pendientes.ToList()
                    .Where(p => DateTime.Today > p.Timestamp + TimeSpan.FromDays(p.Proveedor.DaysDue));
            }
        }

        public static IEnumerable<CuentaBanco> CuentasFor(Empresa e)
        {
            return Proteus.Service<ContabilidadService>()!.All<CuentaBanco>()
                .ToList()
                .Where(p => p.Cuenta.FindRoot().RootParent?.Id == e.Id);
        }

        public async Task RunDepreciaciones()
        {
            foreach (var j in await AllAsync<InventarioFijo>())
            {
                if (!ShouldDepreciar(j)) continue;
                var (current, total) = CalcDeprePeriodos(j);
                var actualValue = Depreciadores[j.Kind.Depreciacion!.DepreciadorGuid].CalcAbsolute(j.ValorInicial, j.ValorResidual, total, current);

            }
        }

        private bool ShouldDepreciar(InventarioFijo inv)
        {
            if (inv.Kind.Depreciacion is null) return false;

            return true;
        }
        private (int current, int total) CalcDeprePeriodos(InventarioFijo inv)
        {
            var minUnit = (TimeUnit) new byte[] { (byte)inv.Kind.LifeUnit, (byte)inv.Kind.Depreciacion!.Periodicity }.Min();
            return (ToUnit(minUnit, inv.Depreciaciones.Count, inv.Kind.LifeUnit), ToUnit(minUnit, inv.Kind.LifeValue, inv.Kind.Depreciacion!.Periodicity));
        }
        private int ToUnit(TimeUnit targetUnit, int value, TimeUnit startingUnit)
        {
            return (startingUnit, targetUnit) switch
            {
                (TimeUnit.Years, TimeUnit.Days) => (int)(value * 365.25),
                (TimeUnit.Years, TimeUnit.Weeks) => value * 52,
                (TimeUnit.Years, TimeUnit.Months) => value * 12,
                (TimeUnit.Months, TimeUnit.Days) => (int)(value * 30.4375),
                (TimeUnit.Months, TimeUnit.Weeks) => (int)(value * (52.0 / 12.0)),
                (TimeUnit.Weeks, TimeUnit.Days) => value * 7,
                _ => throw new InvalidOperationException()
            };




        }
    }
}