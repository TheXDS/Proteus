using System;
using System.Collections.Generic;
using System.Text;
using TheXDS.Proteus.Api;
using TheXDS.Proteus.ContabilidadUi.Pages;
using TheXDS.Proteus.Models;
using TheXDS.Proteus.Plugins;

namespace TheXDS.Proteus.ContabilidadUi.Modules
{
    public class ContabilidadModule : UiModule<ContabilidadService>
    {
        public ContabilidadModule()
        {
            App.UiInvoke(SetupDashboard);
        }

        private void SetupDashboard()
        {
            ModuleDashboard = new ContabMainMenuPage();
        }
    }
}
