using System;
using System.Threading.Tasks;
using System.Windows.Input;
using TheXDS.MCART.Pages;
using TheXDS.Proteus.Pages;

namespace TheXDS.Proteus.DevelModule.ViewModels
{
    public class DbManagementViewModel : ServiceManagementViewModel
    {
        /// <summary>
        /// Obtiene el comando que permite comprobar manualmente la base de
        /// datos.
        /// </summary>
        public ICommand CheckDbCommand { get; }

        public ICommand CheckPendingCommand { get; }

        public ICommand ForcefullySaveCommand { get; }

        /// <summary>
        ///     Obtiene el comando relacionado a la acción DestroyDb.
        /// </summary>
        /// <returns>El comando DestroyDb.</returns>
        public ICommand DestroyDbCommand { get; }

        /// <summary>
        /// Inicializa una nueva instancia de la clase 
        /// <see cref="DbManagementViewModel"/>.
        /// </summary>
        public DbManagementViewModel()
        {
            CheckPendingCommand = MkObsCmd(() => Task.Run(OnCheckPending));
            ForcefullySaveCommand = MkObsCmd(OnForcefullySaveAsync);
            CheckDbCommand = MkObsCmd(OnCheckDbAsync);
            DestroyDbCommand = MkObsCmd(() => Task.Run(OnDestroyDb));
        }

        private void OnCheckPending()
        {
            Proteus.MessageTarget?.Info(SelectedService!.ChangesPending().ToString());
        }
        private Task OnForcefullySaveAsync() => SelectedService!.ForcefullySaveAsync();
        private async Task OnCheckDbAsync()
        {
            var r = await SelectedService!.IsHealthyAsync();
            var msg = $"Servicio saludable: {r}";
            if (Proteus.MessageTarget is { } m)
            {
                (r ? (Action<string>)m.Info : m.Error).Invoke(msg);
            }
            else if (Proteus.AlertTarget is { } a)
            {
                a.Alert(msg);
            }
        }
        private void OnDestroyDb()
        {
            RunSvcMethod("InitializeDatabase", true);
            Proteus.MessageTarget?.Show("Operación de reinicialización", RunSvcMethod("RunSeeders", false)?.ToString() ?? "Desconocido");
            SelectedService!.Reporter?.Done();
        }
    }

    public class TelemetryViewModel : ServiceManagementViewModel
    {
        /// <summary>
        /// Obtiene el comando relacionado a la acción MoreInfo.
        /// </summary>
        /// <returns>El comando MoreInfo.</returns>
        public ICommand MorePluginInfoCommand { get; }
        public ICommand MoreTechInfoCommand { get; }

        public TelemetryViewModel()
        {
            MorePluginInfoCommand = MkObsCmd(() => Task.Run(OnMorePluginInfo));
            MoreTechInfoCommand = MkObsCmd(() => Task.Run(OnMoreTechInfo));
        }

        private void OnMoreTechInfo()
        {
            App.UiInvoke(() => App.RootHost.OpenPage(HostedPage.From(new TypeDetails(SelectedService!.GetType()))));
        }
        private void OnMorePluginInfo()
        {
            App.UiInvoke(() => App.RootHost.OpenPage(HostedPage.From(new PluginDetails() { DataContext = SelectedService })));
        }

    }
}