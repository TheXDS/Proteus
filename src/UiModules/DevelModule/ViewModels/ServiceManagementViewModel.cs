using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TheXDS.MCART;
using TheXDS.MCART.ViewModel;
using TheXDS.Proteus.Api;

namespace TheXDS.Proteus.DevelModule.ViewModels
{
    public abstract class ServiceManagementViewModel : ViewModelBase
    {
        private Service? _selectedService;

        /// <summary>
        /// Enumera los servicios disponibles.
        /// </summary>
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
            get => _selectedService;
            set => Change(ref _selectedService, value);
        }

        private protected bool IsServiceSelected()
        {
            return SelectedService is { };
        }
        private protected object? RunSvcMethod(string method, params object[] args)
        {
            return RunSvcMethod(method, args.ToTypes().ToArray(), args);
        }
        private protected object? RunSvcMethod(string method, Type[] argTypes, object[] args)
        {
            return SelectedService!.GetType().GetMethod(method, BindingFlags.Instance | BindingFlags.NonPublic, null, argTypes, null)!.Invoke(SelectedService, args);
        }        
        private protected ObservingCommand MkObsCmd(Func<Task> task)
        {
            return new ObservingCommand(this, async () => await BusyOp(task))
                .SetCanExecute(IsServiceSelected)
                .RegisterObservedProperty(() => SelectedService);
        }
    }
}