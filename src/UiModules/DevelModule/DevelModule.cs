/*
Copyright © 2017-2019 César Andrés Morgan
Licenciado para uso interno solamente.
*/

using System;
using System.Threading.Tasks;
using TheXDS.MCART.Attributes;
using TheXDS.MCART.PluginSupport.Legacy;
using TheXDS.MCART.Types;
using TheXDS.MCART.Types.Extensions;
using TheXDS.Proteus.Annotations;
using TheXDS.Proteus.Api;
using TheXDS.Proteus.Component;
using TheXDS.Proteus.Dialogs;
using TheXDS.Proteus.Models;
using TheXDS.Proteus.Pages;
using TheXDS.Proteus.Plugins;
using TheXDS.Proteus.Reporting;
using TheXDS.Proteus.DevelModule.Pages;
using static TheXDS.Proteus.Annotations.InteractionType;

[assembly: Name("Devel Module")]

namespace TheXDS.Proteus.DevelModule
{
    public class DevelModule : UiModule
    {
        public DevelModule()
        {
            App.UiInvoke(SetupDashboard);
        }

        private void SetupDashboard()
        {
            ModuleDashboard = Misc.AppInternal.BuildWarning(
                "Este módulo está destinado únicamente a pruebas y al equipo de" +
                " desarrollo de Proteus, por lo que no debe ser distribuido en un" +
                " entorno de producción. César Morgan se absuelve de toda" +
                " responsabilidad por los daños que el uso indebido que este" +
                " módulo pueda causar.");
        }

        [InteractionItem, Essential, InteractionType(AdminTool), Name("Telemetría de servicios")]
        public void OpenServiceInfoPage(object sender, EventArgs e)
        {
            Host.OpenPage<HostedPage<DiagnosticsPage>>();
        }

        [InteractionItem, Essential, InteractionType(AdminTool), Name("Administración de DB")]
        public void OpenDbManagementPage(object sender, EventArgs e)
        {
            Host.OpenPage<HostedPage<DbManagementPage>>();
        }

        [InteractionItem, Essential, InteractionType(AdminTool), Name("Componentes")]
        public void OpenComponentsPage(object sender, EventArgs e)
        {
            Host.OpenPage<HostedPage<ComponentOverviewPage>>();
        }
    }
}
