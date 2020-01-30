using System;
using System.Linq;
using TheXDS.MCART.Attributes;
using TheXDS.MCART.PluginSupport.Legacy;
using TheXDS.MCART.Types.Extensions;
using TheXDS.Proteus.Annotations;
using TheXDS.Proteus.Api;
using TheXDS.Proteus.ContabilidadUi.Pages;
using TheXDS.Proteus.Models;
using TheXDS.Proteus.Pages.Base;
using TheXDS.Proteus.Plugins;
using TheXDS.Proteus.ViewModels;
using TheXDS.Proteus.Widgets;
using TheXDS.Proteus.Reporting;
using System.Windows.Documents;
using System.Collections.Generic;

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
            var fd = ReportBuilder.MakeReport("Balance general");
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
