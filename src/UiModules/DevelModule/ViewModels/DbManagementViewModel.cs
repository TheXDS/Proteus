using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TheXDS.MCART.ViewModel;
using TheXDS.Proteus.Api;
using System.Reflection;
using TheXDS.MCART;

namespace TheXDS.Proteus.DevelModule.ViewModels
{
    public class DbManagementViewModel : ViewModelBase
    {
        private Service? _SelectedService;

        public IEnumerable<Service> Services
        {
            get
            {
                return Proteus.Services ?? Array.Empty<Service>().AsEnumerable();
            }
        }

        /// <summary>
        ///     Obtiene o establece el valor SelectedService.
        /// </summary>
        /// <value>El valor de SelectedService.</value>
        public Service? SelectedService
        {
            get => _SelectedService;
            set => Change(ref _SelectedService, value);
        }

        /// <summary>
        /// Obtiene el comando que permite comprobar manualmente la base de
        /// datos.
        /// </summary>
        public ICommand CheckDbCommand { get; }

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
            CheckDbCommand = new ObservingCommand(this, () => BusyOp(OnCheckDbAsync))
                .SetCanExecute(IsServiceSelected)
                .RegisterObservedProperty(() => SelectedService);

            DestroyDbCommand = new ObservingCommand(this, async () => await BusyOp(Task.Run(OnDestroyDb)))
                .SetCanExecute(IsServiceSelected)
                .RegisterObservedProperty(() => SelectedService);
        }

        private bool IsServiceSelected()
        {
            return SelectedService is { };
        }


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
        private object? RunSvcMethod(string method, params object[] args)
        {
            return SelectedService!.GetType().GetMethod(method, BindingFlags.Instance | BindingFlags.NonPublic, null, args.ToTypes().ToArray(), null)!.Invoke(SelectedService, args);
        }
    }
}