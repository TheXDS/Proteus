using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using TheXDS.MCART.Types.Extensions;
using TheXDS.Proteus.Context;
using TheXDS.Proteus.Models;

namespace TheXDS.Proteus.Api
{
    public class ContabilidadService : Service<ContabilidadContext>
    {
        public static void ForcefullySave()
        {
            Proteus.Service<ContabilidadService>().InternalSaveAsync();
        }
        public AccessControlList? GetList()
        {
            return GetUser<AccessControlList>(this);
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
    }
}