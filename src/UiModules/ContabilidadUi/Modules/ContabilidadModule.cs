using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using TheXDS.MCART.Attributes;
using TheXDS.MCART.Types.Extensions;
using TheXDS.Proteus.Annotations;
using TheXDS.Proteus.Api;
using TheXDS.Proteus.ContabilidadUi.Pages;
using TheXDS.Proteus.Models;
using TheXDS.Proteus.Pages.Base;
using TheXDS.Proteus.Plugins;
using TheXDS.Proteus.Reporting;
using TheXDS.Proteus.ViewModels;
using TheXDS.Proteus.Widgets;
using Microsoft.Win32;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TheXDS.MCART;
using TheXDS.MCART.Resources;
using TheXDS.MCART.Types.Extensions;
using TheXDS.MCART.ViewModel;
using TheXDS.Proteus.Crud;
using TheXDS.Proteus.Crud.Base;
using TheXDS.Proteus.Models.Base;
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
            RegisterLauncher(new Launcher("Catálogo de cuentas", "Administra el catálogo de cuentas para la empresa activa.", OpenAdminCatCuentas), InteractionType.Catalog.NameOf());
            RegisterLauncher(new Launcher("Balance general", "Genera un balance general de la empresa y periodo activos.", MakeBalanceGeneral), InteractionType.Reports.NameOf());

        }

        private async void MakeBalanceGeneral()
        {
            if (!CanOpen()) return;
            var sfd = new SaveFileDialog()
            {
                Filter = "Archivo de Excel (*.xlsx)|*.xlsx|Todos los archivos|*.*"
            };
            if (!(sfd.ShowDialog() ?? false)) return;

            var e = new ExcelPackage();
            var ws = e.Workbook.Worksheets.Add("Balance general");
            ws.Cells[1, 1].Value = ModuleStatus.ActiveEmpresa!.Name + ModuleStatus.ActiveEntidad?.Name.OrNull(", {0}");
            ws.Cells[2, 1].Value = "Balance general";
            ws.Cells[2, 1].Style.Font.Size *= 1.3f;
            ws.Cells[3, 1].Value = string.Format("Reporte generado el {0}", DateTime.Now);
            var lastCol = 0;
            foreach (var i in new[] { "Código", "Nombre de cuenta", "Saldo inicial", "Cambio neto", "Saldo final", "Debe", "Haber" })
            {
                lastCol++;
                ws.Cells[4, lastCol].Value = i;
            }
            ws.Cells[4, 1, 4, lastCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
            var backgroundColor = ws.Cells[4, 1, 4, lastCol].Style.Fill.BackgroundColor;
            var lightSlateGray = Colors.LightSlateGray;
            backgroundColor.SetColor(lightSlateGray);
            for (int j = 1; j < 4; j++)
            {
                ws.Cells[j, 1, j, lastCol].Merge = true;
            }
            var row = 5;
            ProcessCuenta(ws, ModuleStatus.ActiveEmpresa.Activo, ref row);
            ProcessCuenta(ws, ModuleStatus.ActiveEmpresa.Pasivo, ref row);
            ProcessCuenta(ws, ModuleStatus.ActiveEmpresa.Patrimonio, ref row);
            for (int k = 1; k <= lastCol; k++)
            {
                ws.Column(k).AutoFit();
            }
            for (int k = 3; k <= 7; k++)
            {
                ws.Column(k).Style.Numberformat.Format = "_-L* #,##0.00_-;-L* #,##0.00_-;_-L* \" - \"??_-;_-@_-";
            }
            e.SaveAs(new FileInfo(sfd.FileName));
        }

        private void ProcessCuenta(ExcelWorksheet ws, Cuenta c, ref int row)
        {
            ws.Cells[row, 1].Value = c.FullCode;
            ws.Cells[row, 2].Value = c.Name;
            ws.Cells[row, 1, row, 2].Style.Font.Bold = true;
            ws.Cells[row, 3].Value = c.InitialCache;
            ws.Cells[row, 4].Value = c.BalanceCache - c.InitialCache;
            ws.Cells[row, 5].Value = c.BalanceCache;            

            foreach (var j in c.Children)
            {
                row++;
                ProcessCuenta(ws, j, ref row);
            }
            foreach (var j in c.SubCuentas)
            {
                row++;
                ws.Cells[row, 1].Value = j.FullCode;
                ws.Cells[row, 2].Value = j.Name;
                if (j.BalanceCache < 0m)                
                    ws.Cells[row, 7].Value = -j.BalanceCache;
                else
                ws.Cells[row, 6].Value = j.BalanceCache;
            }
        }

        private void Add2Table(Table tbl, Cuenta c, int lvl)
        {
            var r = new List<string>
            {
                $"{new string(' ', lvl)}{c.FullCode}: {c.Name}",
                c.InitialCache.ToString("C"),
                (c.BalanceCache - c.InitialCache).ToString("C")
            };

            if (c.BalanceCache < 0)
            {
                r.Add("");
                r.Add((-c.BalanceCache).ToString("c"));
            }
            
            tbl.AddRow(r).Bold();



            foreach (var j in c.Children)
            {
                Add2Table(tbl, j, lvl + 1);
            }
        }

        private void OpenAdminCatCuentas()
        {
            if (!CanOpen()) return;
            var e = ModuleStatus.ActiveEmpresa!;
            var q = new[] { e.Activo, e.Pasivo, e.Patrimonio }.AsQueryable();
            Host.OpenPage(CrudPage.New<ContabilidadService>("Catálogo de cuentas", q, new[] { typeof(Cuenta), typeof(SubCuenta) }));
        }
    }
}
