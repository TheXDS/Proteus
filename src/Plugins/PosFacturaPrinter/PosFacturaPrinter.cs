using ESC_POS_USB_NET.Printer;
using System;
using System.Collections.ObjectModel;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using TheXDS.MCART;
using TheXDS.MCART.Attributes;
using TheXDS.MCART.Types.Extensions;
using TheXDS.Proteus.Api;
using TheXDS.Proteus.Component;
using TheXDS.Proteus.Component.Attributes;
using TheXDS.Proteus.Models;
using TheXDS.Proteus.Plugins;
using static ESC_POS_USB_NET.Enums.PrinterModeState;

namespace TheXDS.Proteus.PosFacturaPrinter
{
    public enum PosSettings
    {
        [Name("Impresora POS predeterminada"), Default("")] PrinterName,
        [Name("Página de códigos"), Default("iso-8859-1")] PrinterCodePage,
        [Name("Número de columnas"), Default("42"), SettingType(typeof(int))] PrinterColumns,
        [Name("Líneas de búffer post-impresión"), Default("6"), SettingType(typeof(int))] LeadOut,
        [Name("Soporte gráfico"), Default("false"), SettingType(typeof(bool))] Graphics,
        [Name("Dejar que la impresora formatee el texto"), Default("false"), SettingType(typeof(bool))] PrinterFormat,
        [Name("Modo de prueba (Dump a la consola)"), Default("true"), SettingType(typeof(bool))] TestMode
    }

    [Name("Impresora de POS"), Guid("8e4ebc2e-da07-4ecb-abcd-fd0ecc7d7ea1")]
    public class PosSettingsRepository : SettingsRepository<PosSettings>
    {
        public string PrinterName => this[PosSettings.PrinterName].Value;
        public string PrinterCodePage => this[PosSettings.PrinterCodePage].Value;
        public int PrinterColumns => GetAs<int>();
        public int LeadOut => GetAs<int>();
        public bool Graphics => GetAs<bool>();
        public bool PrinterFormat => GetAs<bool>();
        public bool TestMode => GetAs<bool>();
    }

    [Name("Impresora de POS"), Description("Utiliza un sistema de impresión compatible con POS para imprimir la factura.")]
    [Guid("8e4ebc2e-da07-4ecb-abcd-fd0ecc7d7ea1")]
    public class PosFacturaPrinter : FacturaPrintDriver
    {
        private struct ColumnInfo
        {
            public string Header { get; }
            public Func<ItemFactura, string> Selector { get; }
            public int Width { get; }

            public ColumnInfo(string header, int width, Func<ItemFactura, string> selector)
            {
                Header = header;
                Width = width;
                Selector = selector;
            }
        }

        private class ColumnCollection : Collection<ColumnInfo>
        {
            public int LogicalWidth => this.Sum(p => p.Width);

            public void Add(string header, int width, Func<ItemFactura, string> selector)
            {
                Add(new ColumnInfo(header, width, selector));
            }

            public string Get(Func<ColumnInfo,string> text)
            {
                var l = new StringBuilder();
                var w = LogicalWidth;
                foreach (var j in this)
                {
                    var max = j.Width * _maxCols / w;
                    l.Append(text(j).Truncate(max).PadLeft(max));
                }
                return l.ToString();
            }

            public string GetHeader() => Get(j => j.Header);

            public string GetLine(ItemFactura i) => Get(j => j.Selector(i));
        }

        private static readonly ColumnCollection _columns = new ColumnCollection();
        private static readonly PosSettingsRepository _settings = new PosSettingsRepository();
        private static int _maxCols = 0;

        /// <summary>
        /// Inicializa la clase <see cref="PosFacturaPrinter"/>
        /// </summary>
        static PosFacturaPrinter()
        {
            Proteus.RegisterExternalSettingsRepo(_settings);
            _columns.Add("Cant.", 5, p => p.Qty.ToString());
            _columns.Add("Precio", 12, p => p.StaticPrecio.ToString("C"));
            //Columns.Add("ISV", 9, p => p.MontoGravado > 0m ? p.MontoGravado.ToString("C") : "");
            _columns.Add("SubTotal", 16, p => p.SubTotal.ToString("C"));
        }

        private static Printer GetPrinter()
        {
            if (_maxCols == 0) _maxCols = _settings.PrinterColumns;
            
            return new Printer(FacturaService.GetEstation?.Printer
                ?? _settings[PosSettings.PrinterName].Value
                ?? PrinterSettings.InstalledPrinters[0] 
                ?? throw new Exception("No hay ninguna impresora disponible."), 
                _settings[PosSettings.PrinterCodePage].Value);
        }

        private static Printer PrintHeader(string title)
        {
            var e = FacturaService.GetEstation?.Entidad!;
            var p = GetPrinter();

            Action<string> write;
            if (_settings.PrinterFormat)
            {
                p.AlignCenter();
                write = p.Append;
            }
            else
            {
                write = t => Center(p, t);
            }

            write(e.Name);
            if (!e.Banner.IsEmpty()) write(e!.Banner);
            write(e.Address);
            write($"{e.City}, {e.Country}");
            write($"RTN: {e.Id}");
            write($"Tel. {string.Join(", ", e.Phones.AsEnumerable().Select(p => p.Number))}");
            write($"Email {string.Join(", ", e.Emails.AsEnumerable().Select(p => p.Address))}");
            Line(p);
            write(title.ToUpper().Spell());
            Line(p);

            p.AlignLeft();

            return p;
        }

        private static void Line(Printer p, in char c = '-')
        {
            if (_settings.PrinterFormat)
            {
                p.Separator(c);
            }
            else
            {
                p.Append(new string(c, _maxCols));
            }

        }

        private static void Center(Printer p, string text)
        {
            var b = "";
            void Commit() => p.Append($"{new string(' ', (_maxCols - b.Length) / 2)}{b}");

            foreach (var j in text.Split().ToList())
            {
                if (b.Length + j.Length + 1 <= _maxCols)
                {
                    if (b != "") b += " ";
                    b += j;
                }
                else
                {
                    Commit();
                    b = j;
                }
            }
            Commit();
        }

        private static void FooterAndPrint(Printer p)
        {
            p.Append(new string('\n', _settings.LeadOut));
            p.FullPaperCut();
            p.OpenDrawer();
            if (_settings.TestMode)
            {

                return;
            }
            p.PrintDocument();            
        }

        private static void ChkReImpresion(Printer p, Factura f)
        {
            if (f.Impresa)
            {
                Center(p, "ESTA ES UNA RE-IMPRESIÓN");
                Line(p, '=');
            }
        }

        private static void PrintFacturaHeader(Printer p, Factura f)
        {
            p.Append("C.A.I.:");
            p.Append($"{f.CaiRangoParent.Parent.Id}");
            p.Append($"Rango autorizado de facturación:");
            p.Append($"{f.CaiRangoParent.RangoString()}");
            p.Append($"Fecha lim. de emisión: {f.CaiRangoParent.Parent.Void:dd/MM/yyyy}");
            Line(p);
            p.Append($"Factura # {f.FactNum}");
            p.Append($"Fecha de facturación: {f.Timestamp:dd/MM/yyyy}");
            p.Append($"Cliente: {f.Cliente.Name ?? "Consumidor final"}");
            p.Append($"RTN: {f.Cliente?.Rtn ?? "9999-9999-999999"}");
            p.Append("No. Compra exenta:");
            p.Append("No. constancia registro exonerado:");
            p.Append($"{f.Cliente!.Exoneraciones.FirstOrDefault(p => DateTime.Today.IsBetween(p.Timestamp.Date, p.Void.Date + TimeSpan.FromDays(1)))?.Id}");
            p.Append("No. Registro SAG:");
            Line(p, '=');
            ChkReImpresion(p, f);
        }

        private static void PrintTotals(Printer p, Factura f)
        {
            void AddSubt(string label, decimal value) => p.Append($"{$"{label}:".PadLeft(_maxCols - 18)}{value,18:C}");
            void AddSubtGroup(Func<ItemFactura, decimal> selector, string gravFormat, string? exformat = null)
            {
                foreach (var j in f.Items.GroupBy(p => p.StaticIsv ?? 0f))
                {
                    if (j.Key > 0f)
                    {
                        AddSubt(string.Format(gravFormat,j.Key), j.Sum(selector));
                    }
                    else if (exformat is string s)
                    {
                        AddSubt(s, j.Sum(selector));
                    }
                }
            }
            AddSubtGroup(p => p.StaticPrecio * p.Qty, "Importe gravado {0}%", "Importe excento");
            AddSubt("Descuentos otorgados", f.Descuentos);
            AddSubt("Cargos adicionales", f.OtrosCargos);
            AddSubtGroup(p => p.StaticPrecio * p.Qty * (decimal)((p.StaticIsv / 100f) ?? 0f), "I.S.V {0}%");
            p.BoldMode(On);
            AddSubt("TOTAL A PAGAR", f.Total);
            p.BoldMode(Off);
            foreach (var j in f.Payments)
            {
                AddSubt(j.ResolveSource()?.Name ?? "Pago misc.", j.Amount);
            }
            AddSubt("Cambio", -f.Vuelto);
        }

        private static void PrintFacturaFooter(Printer p, Factura f)
        {
            Line(p, '=');
            ChkReImpresion(p, f);
            Action<string> write;
            if (_settings.PrinterFormat)
            {
                p.AlignCenter();
                write = p.Append;
            }
            else
            {
                write = t => Center(p, t);
            }
            if (!f.Impresa)
            {
                write("Gracias por su compra.");
                write($"Atendido por: {FacturaService.GetCajero?.UserEntity?.Name ?? FacturaService.GetCajero?.UserId ?? Proteus.Session?.Id}");
            }
            else
            {
                write("Reimpresión únicamente para propósitos de referencia y archivo.");
            }
            write("Original - Cliente");
            write("CC - Comercio");
            p.AlignLeft();

        }

        public override void PrintFactura(Factura f, IFacturaInteractor? i)
        {
            // Headers
            var p = PrintHeader("factura");
            PrintFacturaHeader(p, f);

            // Items
            p.Append("Descripción");
            p.Append(_columns.GetHeader());
            Line(p);
            foreach (var j in f.Items)
            {
                p.Append($"{j.Item.Name}{(j.StaticIsv.HasValue && j.StaticIsv.Value > 0 ? $" (G {j.StaticIsv}%)":"")}");
                p.Append(_columns.GetLine(j));
            }
            Line(p);

            // Totales
            PrintTotals(p, f);
            if (!f.Notas.IsEmpty())
            {
                Line(p);
                p.Append(f.Notas);
            }

            // Footer
            PrintFacturaFooter(p, f);

            FooterAndPrint(p);
            f.Impresa = true;
        }






        public override void PrintProforma(Factura f, IFacturaInteractor? i)
        {
            var p = PrintHeader("PROFORMA");
            p.BoldMode($"Código de orden: {f.Id:000000}");
            p.Append($"Fecha de generacion: {f.Timestamp:dd/MM/yyyy}");
            p.Code128(f.Id.ToString());
            p.Append($"Cliente: {f.Cliente?.Name ?? "Consumidor final"}");
            p.Append($"RTN: {f.Cliente?.Rtn ?? "9999-9999-999999"}");
            var exonerar = f.Cliente?.Exoneraciones.Any(p => DateTime.Today.IsBetween(p.Timestamp.Date, p.Void.Date + TimeSpan.FromDays(1))) ?? false;
            if (exonerar)
            {
                p.Append("No. constancia registro exonerado:");
                p.Append($"{f.Cliente!.Exoneraciones.FirstOrDefault(p => DateTime.Today.IsBetween(p.Timestamp, p.Void))?.Id}");
            }

            foreach (var j in f.Cliente?.Phones.ToArray() ?? Array.Empty<Phone>())
            {
                p.Append($"Tel.   {j.Number}");
            }
            Line(p,'=');
            p.Append($"{"Cant.",-5}{"Precio",15}{"Subtotal".PadLeft(_maxCols - 20)}");
            Line(p);
            var tot = 0m;
            foreach (var j in f.Items)
            {
                p.AlignLeft();
                p.Append(j.Item.Name);
                var precio = j.Item.Precio;
                if (!exonerar)
                    precio += (j.Item.Precio * (decimal)((j.Item.Isv / 100f) ?? 0f));
                p.AlignRight();
                p.Append($"{j.Qty,-5}{precio,15:C}{(j.Qty * precio).ToString("C").PadLeft(_maxCols - 20)}");
                tot += precio * j.Qty;
            }
            Line(p);
            void AddSubt(string label, decimal value) => p.Append($"{$"{label}:",25}{value.ToString("C").PadLeft(_maxCols - 25)}");
            p.AlignLeft();
            p.Append($"Total de ítems: {f.Items.Sum(j => j.Qty)}");
            p.AlignRight();
            AddSubt("Descuentos", f.Descuentos);
            AddSubt("Otros cargos", f.OtrosCargos);
            AddSubt($"Total a pagar", tot + f.OtrosCargos - f.Descuentos);

            p.AlignLeft();
            if (!f.Notas.IsEmpty())
            {
                Line(p);
                p.Append(f.Notas);
            }
            Line(p, '=');
            p.AlignCenter();
            p.Append("Gracias por preferirnos");
            p.BoldMode("ESTA PROFORMA NO ES UNA FACTURA");
            FooterAndPrint(p);
        }

        public override void PrintCajaOpCut(CajaOp op)
        {
            var p = PrintHeader("CORTE DE CAJA");
            p.Append($"Estación: {op.Estacion.Id}");
            p.Append($"Cajero: {op.Cajero.UserEntity?.Name ?? op.Cajero.UserId}");
            p.Append($"Balance de apertura: {op.OpenBalance:C}");
            if (op.CloseTimestamp.HasValue)
            {
                p.Append($"Corte: {op.CloseTimestamp}");
                p.Append($"Balance de corte: {op.CloseBalance!.Value:C}");
            }
            else
            {
                p.Append("- SESIÓN DE CAJA ACTIVA -");
            }
            p.Append("FACTURAS:");
            p.Append($"Facturas generadas: {op.Facturas.Count}");
            if (op.Facturas.Any())
            {
                Line(p);
                foreach (var j in op.Facturas)
                {
                    p.Append($"Factura #{j.FactNum ?? j.Id.ToString()}");
                    p.Append($"Cliente: {j.Cliente.Name}");
                    if (j.Cliente is {Rtn: string rtn }) p.Append($"RTN: {rtn}");
                    p.Append($"Total: {j.Total:C}");
                    p.Append($"Efectivo: {j.TotalPagadoEfectivo:C}");
                    Line(p);
                }
                p.Append($"Ingreso total: {op.TotalFacturas:C}");
            }
            Line(p,'=');
            p.Append("RETIROS:");
            p.Append($"Eventos de retiro: {op.Drops.Count}");
            if (op.Drops.Any())
            {
                Line(p);
                foreach (var j in op.Drops)
                {
                    p.Append($"Retiro de: {j.Amount:C}");
                    p.Append("Motivo:");
                    p.Append(j.Concept);
                    Line(p);
                }
                p.Append($"Retiro total: {op.TotalDrops:C}");
            }
            FooterAndPrint(p);
        }
    }
}