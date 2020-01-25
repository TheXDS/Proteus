using System.Linq;
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
    public class ContabilidadModule : UiModule<ContabilidadService>
    {
        public static ContabManagerViewModel ModuleStatus { get; private set; }

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
