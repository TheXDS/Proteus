using Microsoft.Win32;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TheXDS.MCART.Attributes;
using TheXDS.MCART.PluginSupport.Legacy;
using TheXDS.MCART.Resources;
using TheXDS.MCART.Types.Extensions;
using TheXDS.Proteus.Annotations;
using TheXDS.Proteus.Api;
using TheXDS.Proteus.ContabilidadUi.Pages;
using TheXDS.Proteus.Models;
using TheXDS.Proteus.Pages;
using TheXDS.Proteus.Pages.Base;
using TheXDS.Proteus.Plugins;
using TheXDS.Proteus.ViewModels;
using TheXDS.Proteus.ViewModels.Base;
using TheXDS.Proteus.Widgets;

namespace TheXDS.Proteus.ContabilidadUi.Modules
{
    [Name("Contabilidad")]
    public class ContabilidadModule : UiModule<ContabilidadService>
    {
        public static ContabManagerViewModel ModuleStatus { get; private set; }
               
        public static bool CanOpen()
        {
            if (ModuleStatus.ActivePeriodo is null)
            {
                Proteus.MessageTarget?.Stop("Debe seleccionar una empresa y/o período contable primero.");
                return false;
            }
            var acl = Proteus.Service<ContabilidadService>()?.GetList();

            if (acl is null)
            {
                Proteus.MessageTarget?.Stop("El usuario no ha sido autorizado para accesar a la funcionalidad del módulo de contabilidad.");
                return false;
            }
            if (acl.UserId == "root") return true;
            foreach (var j in acl.Entries)
            {
                if (j.Empresa == ModuleStatus.ActiveEmpresa && j.Value != AclValue.Allow)
                {
                    Proteus.MessageTarget?.Stop("El usuario actual no tiene permiso de trabajar en la empresa activa.");
                    return false;
                }
                if (j.Entidad == ModuleStatus.ActiveEntidad && j.Value != AclValue.Allow)
                {
                    Proteus.MessageTarget?.Stop("El usuario actual no tiene permiso de trabajar en la entidad activa.");
                    return false;
                }
            }
            if (acl.EmpresaDefault != AclValue.Allow)
            {
                Proteus.MessageTarget?.Stop("El usuario actual no tiene permiso de trabajar en la empresa activa.");
                return false;
            }
            if (acl.EntidadDefault != AclValue.Allow)
            {
                Proteus.MessageTarget?.Stop("El usuario actual no tiene permiso de trabajar en la entidad activa.");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Obtiene un valor que determina si es posible realizar acciones 
        /// administrativas sobre el servicio de contabilidad.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> si el usuario actual tiene acceso a
        /// funciones administrativas del módulo de contabilidad,
        /// <see langword="false"/> en caso contrario.
        /// </returns>
        public static bool IsAdmin()
        {
            var r = Proteus.Service<ContabilidadService>()!.CanRunService(SecurityFlags.Admin) ?? false;
            if (!r) Proteus.MessageTarget?.Stop("Se necesitan permisos administrativos para realizar esta acción.");
            return r;
        }


        [InteractionItem, Essential, InteractionType(InteractionType.Operation), Name("Nueva partida")]
        public void NewPartida(object sender, EventArgs e)
        {
            if (!CanOpen()) return;
            Host.OpenPage(QuickCrudPage.BulkNew<Partida>());
        }

        [InteractionItem, InteractionType(InteractionType.Operation), Name("Cuentas bancarias")]
        public void AdminCuentasBancarias(object sender, EventArgs e)
        {
            if (!CanOpen()) return;
            Host.OpenPage(CrudPage.New<ContabilidadService>("Administrar cuentas bancarias",ContabilidadService.CuentasFor(ModuleStatus.ActiveEmpresa!).AsQueryable(),new[] { typeof(CuentaMovimiento)}));
        }


        [InteractionItem, Essential, InteractionType(InteractionType.AdminTool), Name("Periodo Actual")]
        public void EditPeriodoActual(object sender, EventArgs e)
        {
            if (!CanOpen()) return;
            Host.OpenPage(QuickCrudPage.Edit(ModuleStatus.ActivePeriodo!));
        }

        [InteractionItem, Essential, InteractionType(InteractionType.AdminTool), Name("Cuentas por pagar")]
        public void AdminCtaXPagarTest(object sender, EventArgs e)
        {
            if (!CanOpen()) return;
            Host.OpenPage(CrudPage.New<ContabilidadService, CtaXPagar>(
                $"Cuentas por pagar de {ModuleStatus.ActiveEmpresa}",
                o => o
                .Where(p => p.Empresa.Id == ModuleStatus.ActiveEmpresa!.Id)
                .Where(p=>p.Paid == false)));
        }


        protected override void AfterInitialization()
        {
            base.AfterInitialization();
            App.UiInvoke(SetupDashboard);
        }

        private async void SetupDashboard()
        {
            ModuleDashboard = new ContabMainMenuPage
            {
                DataContext = ModuleStatus = new ContabManagerViewModel()
            };            
            await ModuleStatus.InitViewModel();
        }

        public ContabilidadModule()
        {
            RegisterDictionary("Templates/Templates.xaml");
            RegisterLauncher(new Launcher("Catálogo de cuentas", "Administra el catálogo de cuentas para la empresa activa.", OpenAdminCatCuentas), InteractionType.Catalog);
            RegisterLauncher(new Launcher("Balance general", "Genera un balance general de la empresa y periodo activos.", MakeBalanceGeneral), InteractionType.Reports);
            RegisterLauncher(new Launcher("Cierre de período", "Cierra el periodo actual y creao uno nuevo", DoNewPeriod), InteractionType.Operation);
        }

        private async void DoNewPeriod()
        {
            if (!CanOpen() || !IsAdmin()) return;
            (Reporter ?? Proteus.CommonReporter)?.UpdateStatus("Abriendo nuevo periodo...");
            await Service!.NewPeriod(ModuleStatus.ActiveEmpresa!);
            await ProteusViewModel.FullRefreshVmAsync<ContabManagerViewModel>();
            (Reporter ?? Proteus.CommonReporter)?.Done();
        }

        private async void MakeBalanceGeneral()
        {
            await MakeBalanceGeneral(ModuleStatus.ActivePeriodo);
        }
        public async Task MakeBalanceGeneral(Periodo? periodo)
        {
            if (!CanOpen()) return;
            var sfd = new SaveFileDialog()
            {
                Filter = "Archivo de Excel (*.xlsx)|*.xlsx|Todos los archivos|*.*",
                FileName = $"Balance general - {periodo}.xlsx"
            };
            if (!(sfd.ShowDialog() ?? false)) return;

            var e = new ExcelPackage();

            var ent = await Service!.All<Partida>()
                .Where(p => p.Parent.Id == periodo!.Id)
                .Select(p => p.Entidad)
                .Distinct()
                .ToListAsync();

            var c = 0;
            foreach (var entidad in ent)
            {
                (Reporter ?? Proteus.CommonReporter)?.UpdateStatus(c / ent.Count, $"Procesando todos los movimientos {entidad?.Name.OrNull("de {0}") ?? "generales"} del periodo {periodo}...");
                await ProcessEntidad(e, entidad, periodo!);
            }

            (Reporter ?? Proteus.CommonReporter)?.UpdateStatus("Guardando reporte...");
            await Task.Run(() => e.SaveAs(new FileInfo(sfd.FileName)));
            (Reporter ?? Proteus.CommonReporter)?.Done();
        }

        private void ProcessDivisa(ExcelPackage e, Entidad? entidad, IGrouping<Divisa, Periodo.PeriodoContabTreeItem> tree, Periodo periodo)
        {
            var symbol = tree.Key?.Region.CurrencySymbol ?? System.Globalization.RegionInfo.CurrentRegion.CurrencySymbol;
            var ws = e.Workbook.Worksheets.Add($"{entidad?.Name ?? "Balance general"}{tree.Key?.Region.CurrencySymbol.OrNull(" divisa {0}")}");
            ws.Cells[1, 1].Value = ModuleStatus.ActiveEmpresa!.Name + entidad?.Name.OrNull(", {0}");
            ws.Cells[2, 1].Value = $"Balance general - {periodo}{tree.Key?.Name.OrNull(" en divisa {0}")}";
            ws.Cells[2, 1].Style.Font.Size *= 1.3f;
            ws.Cells[3, 1].Value = string.Format("Reporte generado el {0}", DateTime.Now);
            var lastCol = 0;
            foreach (var i in new[] { "Código", "Nombre de cuenta" })
            {
                lastCol++;
                ws.Cells[4, lastCol].Value = i;
            }
            var row = 5;
            foreach (var k in tree)
            {
                ProcessCuenta(ws, k, ref row, 3, ref lastCol);
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
                if (j % 2 == 0)
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

        private async Task ProcessEntidad(ExcelPackage e, Entidad? entidad, Periodo periodo)
        {
            foreach (var j in await Task.Run(periodo.GetContabTree))
            {
                ProcessDivisa(e, entidad, j, periodo);
            }
        }

        private void ProcessCuenta(ExcelWorksheet ws, Periodo.PeriodoContabTreeItem c, ref int row, int lvl, ref int maxlvl)
        {
            if (lvl > maxlvl) maxlvl = lvl;

            ws.Cells[row, 1].Value = c.FullCode;
            ws.Cells[row, 2].Value = c.DisplayName;
            ws.Cells[row, 1, row, 2].Style.Font.Bold = c.Bold;
            if (c.Value < 0m)
                ws.Cells[row, lvl + 1].Value = -c.Value;
            else
                ws.Cells[row, lvl].Value = c.Value;

            foreach (var j in c.Children)
            {
                row++;
                ProcessCuenta(ws, j, ref row, lvl + 2, ref maxlvl);
            }
        }

        private void OpenAdminCatCuentas()
        {
            if (!CanOpen()) return;
            var e = ModuleStatus.ActiveEmpresa!;
            var q = new[] { e.Activo, e.Pasivo, e.Patrimonio, e.Ingresos, e.Costos, e.Gastos }.AsQueryable();
            Host.OpenPage(CrudPage.New<ContabilidadService>("Catálogo de cuentas", q, new[] { typeof(Cuenta), typeof(SubCuenta) }));
        }
    }
}
