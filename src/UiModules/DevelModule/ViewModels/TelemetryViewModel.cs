using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using TheXDS.MCART.Pages;
using TheXDS.MCART.Types;
using TheXDS.Proteus.Api;
using TheXDS.Proteus.Pages;

namespace TheXDS.Proteus.DevelModule.ViewModels
{
    public class TelemetryViewModel : ServiceManagementViewModel
    {
        /// <summary>
        /// Obtiene el comando relacionado a la acción MoreInfo.
        /// </summary>
        /// <returns>El comando MoreInfo.</returns>
        public ICommand MorePluginInfoCommand { get; }
        public ICommand MoreTechInfoCommand { get; }
        public ICommand ElevateCommand { get; }
        public ICommand RevokeElevationCommand { get; }


        public TelemetryViewModel()
        {
            MorePluginInfoCommand = MkAsyncObsCmd(OnMorePluginInfo);
            MoreTechInfoCommand = MkAsyncObsCmd(OnMoreTechInfo);
            ElevateCommand = MkAsyncObsCmd(OnElevate);
            RevokeElevationCommand = MkAsyncObsCmd(OnRevokeElevation);
        }

        private void OnRevokeElevation()
        {
            SelectedService!.Revoke();
        }

        private void OnElevate()
        {
            var flg = (SecurityFlags)(1 << 30);
            if (SelectedService.CanRunService(flg) ?? false)
            {
                Proteus.MessageTarget?.Info("El servicio no requiere elevación para la sesión actual (¿Es usted root?)");
                return;
            }
            SelectedService!.Elevate(flg);
            Notify("SelectedService.Session");
        }

        private void OnMoreTechInfo()
        {
            App.UiInvoke(() => App.RootHost.OpenPage(HostedPage.From(new TypeDetails(SelectedService!.GetType()))));
        }

        private void OnMorePluginInfo()
        {
            App.UiInvoke(() => App.RootHost.OpenPage(HostedPage.From(new PluginDetails() { DataContext = SelectedService })));
        }

        public static IEnumerable<NamedObject<ElevationBehavior>> Behaviors => NamedObject<ElevationBehavior>.FromEnum();
    }
}