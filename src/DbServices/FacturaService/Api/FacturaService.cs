﻿using System;
using System.Collections.Generic;
using System.Text;
using TheXDS.Proteus.Context;
using TheXDS.Proteus.Models;
using TheXDS.Proteus.Plugins;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using TheXDS.MCART;
using TheXDS.MCART.Attributes;
using TheXDS.MCART.Security.Password;
using TheXDS.MCART.Types.Extensions;
using static TheXDS.Proteus.Proteus;
using TheXDS.Proteus.Reporting;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using TheXDS.Proteus.Component;
using MigraDoc.Rendering.Printing;
using System.Drawing.Printing;

namespace TheXDS.Proteus.Api
{
    public class FacturaService : Service<FacturaContext>
    {
        public static List<PaymentSource> PaymentSources { get; } = Objects.FindAllObjects<PaymentSource>().ToList();

        public static Estacion GetEstation => GetStation<Estacion>();
        public static Cajero GetCajero => GetUser<Cajero>();
        public static CajaOp GetCajaOp
        {
            get
            {
                var c = GetCajero;
                var s = GetEstation;
                if (new object[] { c, s }.IsAnyNull()) return null;
                return Service<FacturaService>().FirstOrDefault<CajaOp>(p => p.CloseTimestamp == null && p.Cajero.Id == c.Id && p.Estacion.Id == s.Id);
            }
        }
        public static string CajeroName => Proteus.Session?.Name ?? "Estación de facturación";
        public static CaiRango CurrentRango
        {
            get
            {
                return GetEstation.RangosAsignados
                    .OrderBy(p => FreeCorrelCount(p))
                    .FirstOrDefault(p => IsRangoOpen(p));
            }
        }
        public static bool IsThisStation => !(GetEstation is null);
        public static bool IsThisCajero => !(GetCajero is null);
        public static bool IsCajaOpOpen => !(GetCajaOp is null);
        public static IQueryable<CaiRango> UnassignedRangos => Service<FacturaService>()!.All<CaiRango>().Where(p => p.AssignedTo == null);







        public static bool IsRangoOpen(CaiRango rango)
        {
            return !(NextCorrel(rango) is null);
        }
        public static int? NextCorrel(CaiRango rango)
        {
            var l =rango.GetFreeCorrels();
            l.Sort();
            return l.FirstOrDefault();
        }
        public static int? NextCorrel() => NextCorrel(CurrentRango);
        public static int FreeCorrelCount(CaiRango rango) => rango.GetFreeCorrels().Count;
        public static int FreeCorrelCount() => CurrentRango.GetFreeCorrels().Count;
        public static string? GetFactNum(Factura f)
        {
            if (f is null) return null;
            CaiRango r;
            int? c;
            if (f.CaiRangoParent is null)
            {
                r = CurrentRango;
                c = NextCorrel(r);
            }
            else
            {
                r = f.CaiRangoParent;
                c = f.Correlativo;
            }
            if (r is null || c is null) return null;
            return $"{r.NumLocal:000}-{r.NumCaja:000}-{r.NumDocumento:00}-{c:00000000}";
        }
        public static void AddFactura(Factura f, bool register, IFacturaInteractor i)
        {
            if (register)
            {
                RegisterFactura(f, i);
            }
            GetCajaOp.Facturas.Add(f);
        }
        public static void RegisterFactura(Factura f, IFacturaInteractor i)
        {
            f.CaiRangoParent = CurrentRango;
            f.Correlativo = NextCorrel(f.CaiRangoParent) ?? 1;
            PrintFactura(f, i);
        }

        public static void PrintFactura(Factura f, IFacturaInteractor i)
        {
            var doc = DocumentBuilder.CreateDocument();
            var section = AddFacturaHeader(doc, f);
            AddItemsTable(section, i);

            using var p = new MigraDocPrintDocument(doc)
            {
                PrinterSettings = new PrinterSettings()
            };
            p.Print();
            f.Impresa = true;
        }

        private static Section AddFacturaHeader(Document doc, Factura f)
        {
            var retVal = doc.NewSection("FACTURA");
            retVal.AddParagraph(f.FactNum);
            return retVal;
        }

        private static Table AddItemsTable(Section section, IFacturaInteractor i)
        {
            var c = 0;
            var cols = new[]
            {
                new FacturaColumn("#", _ => (++c).ToString()),
                new FacturaColumn("Item", f => f.Item.Name, 4.0),
            }.Concat(i?.ExtraColumns ?? Array.Empty<FacturaColumn>()).Concat(new[]
            {
                new FacturaColumn("Cant.", f => f.Qty.ToString()),
                new FacturaColumn("Precio", f => f.StaticPrecio.ToString(), 2.0, true),
                new FacturaColumn("Descuentos", f => f.StaticDescuento.ToString(), 2.0, true),
                new FacturaColumn("Sub Total", f => f.SubTFinal.ToString(), 2.0, true),

            }).ToList();
            var colsTot = cols.Sum(p => p.RelaSize);
            var tblWidth = 18.5;

            var tbl = section.AddTable();
            foreach (var j in cols)
            {
                tbl.AddColumn(new Unit(j.RelaSize * tblWidth / colsTot, UnitType.Centimeter))
                    .Format.Alignment = j.Currency ? ParagraphAlignment.Right : ParagraphAlignment.Left;
            }
            var row = tbl.AddRow();
            foreach (var j in cols)
            {
                row.Cells[c++].AddParagraph(j.Header).Format.Font.Bold = true;
            };
            c = 0; // la función lambda para la primera columna necesita este valor.
            return tbl;
        }
    }
}
