using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using TheXDS.MCART.Attributes;
using TheXDS.MCART.Types;
using TheXDS.MCART.Types.Extensions;
using TheXDS.Proteus.Api;
using TheXDS.Proteus.Models;
using TheXDS.Proteus.Models.Base;
using static TheXDS.Proteus.Proteus;

namespace TheXDS.Proteus.Component
{
    [Name("Creación de Tarjetas de regalo")]
    [Guid("93b7f8f8-398f-4591-9f01-a8189b631157")]
    public class GiftCardCreationAutomator : IFacturableAutomation
    {
        public void OnFacturate(Factura f, Facturable item, int qty)
        {
            while (qty-- > 0)
            {
                Service<FacturaService>()!.Add(new GiftCard { Amount = item.Precio });
            }
        }
    }

    [Name("Rebaja manual de S/N de inventario")]
    [Guid("dca27907-1101-4d45-8c98-2938b17b0dcb")]
    public class ManualSnRebajador : IFacturableAutomation
    {
        public bool OnFacturateSuccess(Factura f, Facturable item, int qty)
        {
            var l = new HashSet<SerialNum>(new IdComparer<SerialNum>());
            while (l.Count < qty)
            {
                string s = null!;
                if (InputTarget!.Get($"Ingrese o escanee el S/N a rebajar del inventario (ítem: {item}, {l.Count}/{qty})", ref s) && !s.IsEmpty())
                {
                    if (Service<FacturaService>()!.Get<SerialNum>(s) is { } b && b.Parent.Item == item)
                    {
                        l.Add(b);
                    }
                    else
                    {
                        MessageTarget?.Stop("El S/N no existe, o no corresponde al ítem esperado.");
                    }
                }
                else
                {
                    MessageTarget?.Stop("Se ha cancelado la rebaja de inventario.");
                    return false;
                }
            }

            foreach (var i in l)
            {
                i.Warranty = new Warranty
                {
                    Cliente = f.Cliente,
                    Timestamp = f.Timestamp,
                    Void = i.Parent.CalcFrom()
                };
                i.Sold = f.Timestamp;

            }
            return true;
        }
    }

    [Name("Rebaja automática de inventario")]
    [Guid("ee28b968-79be-4b61-ac3f-8f9489855760")]
    public class AutomaticInvRebajador : IFacturableAutomation
    {
        private static Batch? GetBatchFor(Producto prod)
        {
            var e = FacturaService.GetEstation!.Id;
            return Service<FacturaService>()!
                .All<EstacionBodega>()
                .Where(p => p.Estacion.Id == e)
                .Select(p => p.Bodega)
                .SelectMany(p => p.Batches)
                .Where(q => q.Item.Id == prod.Id)
                .ToList()
                .Where(q => q.Qty > 0)
                .OrderBy(q => q.Lote?.Manufactured)
                .FirstOrDefault();
        }

        private static bool ProcessInvBajaItem(Producto p, int qty, AutoDictionary<Batch, int> op)
        {
            while (qty > 0)
            {
                var b = GetBatchFor(p);
                if (b is null)
                {
                    MessageTarget?.Stop($"No hay suficientes existencias para completar la venta. Faltan {qty} unidades de {p.Name}");
                    break;
                }
                if (b.Qty > qty)
                {
                    op[b] += qty;
                    qty = 0;
                }
                else
                {
                    qty -= b.Qty;
                    op[b] += b.Qty;
                }
            }
            return qty == 0;
        }

        public bool OnFacturateSuccess(Factura f, Facturable item, int qty)
        {
            if (!FacturaService.GetEstation!.Bodegas.Any())
            {
                MessageTarget?.Stop("Esta estación no tiene permiso para facturar productos: No hay establecida una bodega de salida.");
                return false;
            }

            var op = new AutoDictionary<Batch, int>();
            switch (item)
            {
                case Producto p:
                    if (!ProcessInvBajaItem(p, qty, op)) return false;
                    break;
                case Paquete paquete:
                    foreach (var k in paquete.Children.OfType<Producto>())
                    {
                        if (!ProcessInvBajaItem(k, qty, op)) return false;
                    }
                    break;
            }
            var sb = new StringBuilder();
            foreach (var j in op)
            {
                sb.AppendLine($"Salida de Batch {j.Key.Id}: {j.Key.RebajarVenta(j.Value, f)}");
            }

            if (op.Any()) MessageTarget?.Info(sb.ToString());

            return true;
        }
    }
}