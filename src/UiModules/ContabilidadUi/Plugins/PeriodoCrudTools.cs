/*
Copyright © 2017-2020 César Andrés Morgan
Licenciado para uso interno solamente.
*/

using Microsoft.Win32;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TheXDS.MCART.Resources;
using TheXDS.MCART.Types.Extensions;
using TheXDS.MCART.ViewModel;
using TheXDS.Proteus.Api;
using TheXDS.Proteus.ContabilidadUi.Modules;
using TheXDS.Proteus.Models;
using TheXDS.Proteus.Plugins;
using TheXDS.Proteus.ViewModels.Base;
using TheXDS.Proteus.Widgets;
using static TheXDS.MCART.ReflectionHelpers;

namespace TheXDS.Proteus.ContabilidadUi.Plugins
{
    public class PeriodoCrudTools : CrudTool<Periodo>
    {
        public PeriodoCrudTools() : base(CrudToolVisibility.EditAndSelected)
        {
        }

        public override IEnumerable<Launcher> GetLaunchers(IEnumerable<Type> models, ICrudViewModel? vm)
        {
            yield return new Launcher(
                "Balance general",
                "Genera un balance general del periodo.",
                GetMethod<ContabilidadModule, Func<Periodo?, Task>>(p => p.MakeBalanceGeneral).Name,
                new SimpleCommand(async () => await App.Module<ContabilidadModule>()!.MakeBalanceGeneral(vm?.Selection as Periodo)));

            //yield return new Launcher(
            //    "Exportar libro diario",
            //    "Exporta el libro diario de partidas a un documento de Microsoft Excel.",
            //    GetMethod<PeriodoCrudTools, Func<Periodo?, Task>>(p => p.SaveLibroDiario).Name,
            //    new SimpleCommand(async () => await SaveLibroDiario(vm?.Selection as Periodo ?? ContabilidadModule.ModuleStatus.ActivePeriodo)));
        }
        /*
        private async Task SaveLibroDiario(Periodo? periodo)
        {
            var sfd = new SaveFileDialog()
            {
                Filter = "Archivo de Excel (*.xlsx)|*.xlsx|Todos los archivos|*.*",
                FileName = $"Libro diario - {periodo}.xlsx"
            };
            if (!(sfd.ShowDialog() ?? false)) return;
            var e = new ExcelPackage();
            var ent = await Proteus.Service<ContabilidadService>()!.All<Partida>()
                .Where(p => p.Parent.Id == periodo!.Id)
                .Select(p => p.Entidad)
                .Distinct()
                .ToListAsync();

            var c = 0;
            var rptr = App.Module<ContabilidadModule>()!.Reporter;
            foreach (var entidad in ent)
            {

                (rptr ?? Proteus.CommonReporter)?.UpdateStatus(c / ent.Count, $"Procesando todas las partidas {entidad?.Name.OrNull("de {0}")} del periodo {periodo}...");
                await ProcessEntidad(e, entidad, periodo!);
            }

            (rptr ?? Proteus.CommonReporter)?.UpdateStatus("Guardando reporte...");
            await Task.Run(() => e.SaveAs(new FileInfo(sfd.FileName)));
            (rptr ?? Proteus.CommonReporter)?.Done();

        }
        private async Task ProcessEntidad(ExcelPackage e, Entidad? entidad, Periodo periodo)
        {
            foreach (var j in await Proteus.Service<ContabilidadService>()!.All<Partida>().Where(p => p.Parent.Id == periodo.Id).GroupBy(p=>p.p).ToListAsync())
            {
                ProcessDivisa(e, entidad, j, periodo);
            }
        }
        private void ProcessDivisa(ExcelPackage e, Entidad? entidad, IGrouping<Divisa, Periodo.PeriodoContabTreeItem> tree, Periodo periodo)
        {
            var symbol = tree.Key?.Region.CurrencySymbol ?? System.Globalization.RegionInfo.CurrentRegion.CurrencySymbol;
            var ws = e.Workbook.Worksheets.Add($"{entidad?.Name ?? "Libro diario"}{tree.Key?.Region.CurrencySymbol.OrNull(" divisa {0}")}");
            ws.Cells[1, 1].Value = ContabilidadModule.ModuleStatus.ActiveEmpresa!.Name + entidad?.Name.OrNull(", {0}");
            ws.Cells[2, 1].Value = $"Libro diario - {periodo}{tree.Key?.Name.OrNull(" en divisa {0}")}";
            ws.Cells[2, 1].Style.Font.Size *= 1.3f;
            ws.Cells[3, 1].Value = string.Format("Reporte generado el {0}", DateTime.Now);
            var lastCol = 0;
            foreach (var i in new[] { "Fecha", "Descripción", "Centro de costos" })
            {
                lastCol++;
                ws.Cells[4, lastCol].Value = i;
            }
            var row = 5;
            foreach (var k in tree)
            {
                ProcessCuenta(ws, k, ref row, 4);
                row++;
            }
            lastCol++;
            ws.Cells[4, 1, 4, lastCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
            var backgroundColor = ws.Cells[4, 1, 4, lastCol].Style.Fill.BackgroundColor;
            var lightSlateGray = Colors.LightSlateGray;
            backgroundColor.SetColor(lightSlateGray);
            for (int j = 1; j < 4; j++)
            {
                ws.Cells[j, 1, j, lastCol].Merge = true;
            }
            for (int j = 3; j <= lastCol; j++)
            {
                if (j % 2 != 0)
                {
                    ws.Cells[4, j].Value = "Haber";
                }
                else
                {
                    ws.Column(j).Style.Border.Left.Style = ExcelBorderStyle.Medium;
                    ws.Cells[4, j].Value = "Debe";
                }
                ws.Column(j).Style.Numberformat.Format = $"_-{symbol}* #,##0.00_-;-{symbol}* #,##0.00_-;_-{symbol}* \" - \"??_-;_-@_-";
            }
            for (int j = 1; j <= lastCol; j++)
            {
                ws.Column(j).AutoFit();
            }
        }
        private void ProcessPartida(ExcelWorksheet ws, Partida c, ref int row)
        {
            ws.Cells[row, 1].Value = c.Timestamp;
            ws.Cells[row, 2].Value = c.Description;
            ws.Cells[row, 1, row, 2].Style.Font.Bold = true;
            foreach (var j in c.Movimientos)
            {
                row++;
                ws.Cells[row, 2].Value = j.Cuenta.ToString();
                ws.Cells[row, 3].Value = j.CostCenter?.ToString();
                if (j.RawValue < 0m)
                    ws.Cells[row, 4].Value = -j.RawValue;
                else
                    ws.Cells[row, 5].Value = j.RawValue;
            }
        }

        */
    }
}
