using TheXDS.Proteus.Api;
using TheXDS.Proteus.ContabilidadUi.Pages;
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
            ModuleDashboard = new ContabMainMenuPage
            {
                DataContext = ModuleStatus = new ContabManagerViewModel()
            };            
            await ModuleStatus.InitViewModel();
        }
    }
}
