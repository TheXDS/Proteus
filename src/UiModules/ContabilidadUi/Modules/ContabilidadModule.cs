using System.Linq;
using TheXDS.MCART.Attributes;
using TheXDS.MCART.Types.Extensions;
using TheXDS.Proteus.Annotations;
using TheXDS.Proteus.Api;
using TheXDS.Proteus.ContabilidadUi.Pages;
using TheXDS.Proteus.Models;
using TheXDS.Proteus.Pages.Base;
using TheXDS.Proteus.Plugins;
using TheXDS.Proteus.ViewModels;

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
            RegisterLauncher(new Widgets.Launcher("Catálogo de cuentas", "Administra el catálogo de cuentas para la empresa activa.", () =>
            {
                if (!(ModuleStatus.ActiveEmpresa is { } e)) return;
                var q = new[]
                {
                    e.Activo,
                    e.Pasivo,
                    e.Patrimonio
                }.AsQueryable();

                Host.OpenPage(CrudPage.New<ContabilidadService>("Catálogo de cuentas", q, new[] { typeof(Cuenta), typeof(SubCuenta) }));
            }), InteractionType.Catalog.NameOf());

            RegisterDictionary("Templates/Templates.xaml");


            ModuleDashboard = new ContabMainMenuPage
            {
                DataContext = ModuleStatus = new ContabManagerViewModel()
            };            
            await ModuleStatus.InitViewModel();
        }
    }
}
