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
        [Name("Dejar que la impresora formatee el texto"), Default("false"), SettingType(typeof(bool))] PrinterFormat
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
    }

    [Name("Impresora de POS"), Description("Utiliza un sistema de impresión compatible con POS para imprimir la factura.")]
    [Guid("8e4ebc2e-da07-4ecb-abcd-fd0ecc7d7ea1")]
    public class PosFacturaPrinter : FacturaPrintDriver
    {
        private class ColumnInfo
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

            public string GetHeader()
            {
                var l = new StringBuilder();
                var w = LogicalWidth;
                foreach (var j in this)
                {
                    var max = j.Width * _maxCols / w;
                    l.Append(j.Header.Truncate(max).PadLeft(max));
                }
                return l.ToString();
            }

            public string GetLine(ItemFactura i)
            {
                var l = new StringBuilder();
                var w = LogicalWidth;
                foreach (var j in this)
                {
                    var max = j.Width * _maxCols / w;
                    l.Append(j.Selector(i).Truncate(max).PadLeft(max));
                }
                return l.ToString();
            }
        }

        private static readonly PosSettingsRepository _settings = new PosSettingsRepository();
        private static int _maxCols = 0;
        private static CultureInfo ci = CultureInfo.CreateSpecificCulture("es-HN");

        /// <summary>
        ///     Inicializa la clase <see cref="PosFacturaPrinter"/>
        /// </summary>
        static PosFacturaPrinter()
        {
            Proteus.RegisterExternalSettingsRepo(_settings);
            Columns.Add(new ColumnInfo("Cant.", 5, p => p.Qty.ToString()));
            Columns.Add(new ColumnInfo("Precio", 12, p => p.StaticPrecio.ToString("C")));
            Columns.Add(new ColumnInfo("ISV", 8, p => p.StaticIsv.HasValue && p.StaticIsv.Value > 0 ? p.MontoGravado.ToString("C"):""));
            Columns.Add(new ColumnInfo("SubTotal", 15, p => p.SubTotal.ToString("C")));
        }

        private static Printer GetPrinter()
        {
            if (_maxCols == 0) _maxCols = int.TryParse(_settings[PosSettings.PrinterColumns].Value, out var c) ? c : 42;
            
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

            if (_settings.PrinterFormat)
            {
                p.AlignCenter();
                p.Append(e.Name);
                if (!e.Banner.IsEmpty()) p.Append(e!.Banner);
                p.Append(e.Address);
                p.Append($"{e.City}, {e.Country}");
                p.Append($"RTN: {e.Id}");
                p.Append($"Tel. {string.Join(", ", e.Phones.AsEnumerable().Select(p=>p.Number))}");
                p.Append($"Email {string.Join(", ", e.Emails.AsEnumerable().Select(p => p.Address))}");
                Line(p);
                p.Append(title.ToUpper().Spell());
                Line(p);
                p.AlignLeft();
            }
            else
            {
                Center(p, e.Name);
                if (!e.Banner.IsEmpty()) Center(p, e!.Banner);
                Center(p, e.Address);
                Center(p, $"{e.City}, {e.Country}");
                Center(p, $"RTN: {e.Id}");
                Center(p, $"Tel. {string.Join(", ", e.Phones.AsEnumerable().Select(p => p.Number))}");
                Center(p, $"Email {string.Join(", ", e.Emails.AsEnumerable().Select(p => p.Address))}");
                Line(p);
                Center(p, title.ToUpper().Spell());
                Line(p);
            }
            return p;
        }

        private static void Line(Printer p, in char c = '-')
        {
            p.Append(new string(c, _maxCols));
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
            p.NewLines(_settings.LeadOut);
            p.FullPaperCut();
            p.OpenDrawer();
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

        private static readonly ColumnCollection Columns = new ColumnCollection();

        public override void PrintFactura(Factura f, IFacturaInteractor? i)
        {
            var p = PrintHeader("factura");
            PrintFacturaHeader(p, f);
            p.Append("Descripción");
            p.Append(Columns.GetHeader());
            Line(p);
            foreach (var j in f.Items)
            {
                p.Append($"{j.Item.Name}{(j.StaticIsv.HasValue && j.StaticIsv.Value > 0 ? $" (G {j.StaticIsv}%)":"")}");
                p.Append(Columns.GetLine(j));
            }
            Line(p);

            void AddSubt(string label, decimal value) => p.Append($"{$"{label}:".PadLeft(_maxCols - 20)}{value.ToString("C", ci),20}");

            AddSubt("Subtotal", f.SubTotal);
            AddSubt("Total ISV", f.SubTGravable);
            AddSubt("Subtotal Gravado", f.SubTGravado);
            AddSubt("Descuentos", f.Descuentos);

            p.BoldMode(On);
            AddSubt("TOTAL", f.Total);
            p.BoldMode(Off);

            foreach (var j in f.Payments)
            {
                AddSubt(j.ResolveSource()?.Name ?? "Pago misc.", j.Amount);
            }
            AddSubt("Cambio", -f.Vuelto);
            if (!f.Notas.IsEmpty())
            {
                Line(p);
                p.Append(f.Notas);
            }

            Line(p, '=');
            ChkReImpresion(p, f);

            if (_settings.PrinterFormat) { 
                p.AlignCenter();
                if (!f.Impresa)
                {
                    p.Append("Gracias por su compra.");
                    p.Append($"Atendido por: {FacturaService.GetCajero?.UserEntity?.Name ?? FacturaService.GetCajero?.UserId ?? Proteus.Session?.Id}");
                }
                else
                {
                    p.Append("Reimpresión únicamente para propósitos de referencia y archivo.");
                }
                p.Append("Original - Cliente");
                p.Append("CC - Comercio");
            }
            else
            {
                if (!f.Impresa)
                {
                    Center(p, "Gracias por su compra.");
                    Center(p, $"Atendido por: {FacturaService.GetCajero?.UserEntity?.Name ?? FacturaService.GetCajero?.UserId ?? Proteus.Session?.Id}");

                }
                else
                {
                    Center(p, "Reimpresión únicamente para propósitos de referencia y archivo.");
                }
                Center(p, "Original - Cliente");
                Center(p, "CC - Comercio");
            }
            FooterAndPrint(p);
            f.Impresa = true;
        }

        public override void PrintProforma(Factura f, IFacturaInteractor? i)
        {
            var ci = System.Globalization.CultureInfo.CreateSpecificCulture("es-HN");
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
                p.Append($"{j.Qty,-5}{precio.ToString("C", ci),15}{(j.Qty * precio).ToString("C", ci).PadLeft(_maxCols - 20)}");
                tot += precio * j.Qty;
            }
            Line(p);
            void AddSubt(string label, decimal value) => p.Append($"{$"{label}:",25}{value.ToString("C", ci).PadLeft(_maxCols - 25)}");
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
            var ci = System.Globalization.CultureInfo.CreateSpecificCulture("es-HN");
            var p = PrintHeader("CORTE DE CAJA");
            p.Append($"Estación: {op.Estacion.Id}");
            p.Append($"Cajero: {op.Cajero.UserEntity?.Name ?? op.Cajero.UserId}");
            p.Append($"Balance de apertura: {op.OpenBalance.ToString("C", ci)}");
            if (op.CloseTimestamp.HasValue)
            {
                p.Append($"Corte: {op.CloseTimestamp}");
                p.Append($"Balance de corte: {op.CloseBalance!.Value.ToString("C", ci)}");
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
                    p.Append($"Total: {j.Total.ToString("C", ci)}");
                    p.Append($"Efectivo: {j.TotalPagadoEfectivo.ToString("C", ci)}");
                    Line(p);
                }
                p.Append($"Ingreso total: {op.TotalFacturas.ToString("C", ci)}");
            }
            Line(p,'=');
            p.Append("RETIROS:");
            p.Append($"Eventos de retiro: {op.Drops.Count}");
            if (op.Drops.Any())
            {
                Line(p);
                foreach (var j in op.Drops)
                {
                    p.Append($"Retiro de: {j.Amount.ToString("C", ci)}");
                    p.Append("Motivo:");
                    p.Append(j.Concept);
                    Line(p);
                }
                p.Append($"Retiro total: {op.TotalDrops.ToString("C", ci)}");
            }
            FooterAndPrint(p);
        }
    }
}