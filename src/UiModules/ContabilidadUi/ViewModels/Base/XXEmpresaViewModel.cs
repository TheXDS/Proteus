using System;
using System.Collections.Generic;
using System.Linq;
using TheXDS.MCART.Types;
using TheXDS.MCART.ViewModel;
using TheXDS.Proteus.Models;
using TheXDS.Proteus.Models.Base;
using TheXDS.Proteus.ViewModels.Base;

namespace TheXDS.Proteus.ContabilidadUi.ViewModels.Base
{
    public class XXEmpresaViewModel<T> : ViewModel<T> where T : ModelBase, IXXEmpresa, new()
    {
        private ISearchViewModel? _vm;
        private Empresa? _selectedEmpresa;

        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// <see cref="XXEmpresaViewModel{T}"/>.
        /// </summary>
        public XXEmpresaViewModel()
        {
        }

        public ObservableListWrap<ModelBase> CurrentSubCuentas { get; } = new ObservableListWrap<ModelBase>();

        public ObservableListWrap<ModelBase> GetCurrentSubCuentas(ISearchViewModel? vm)
        {
            _vm ??= vm;
            return CurrentSubCuentas;
        }

        /// <summary>
        ///     Obtiene o establece el valor SelectedEmpresa.
        /// </summary>
        /// <value>El valor de SelectedEmpresa.</value>
        public Empresa SelectedEmpresa
        {
            get => _selectedEmpresa ?? Entity.Empresa;
            set
            {
                if (!Change(ref _selectedEmpresa, value)) return;
                Entity.Empresa = value;
                Notify("Empresa");
                Notify("Entity.Empresa");
                CurrentSubCuentas
                    .Substitute(Flatten(value?.Activo)
                    .Concat(Flatten(value?.Pasivo))
                    .Concat(Flatten(value?.Patrimonio))
                    .Concat(Flatten(value?.Ingresos))
                    .Concat(Flatten(value?.Costos))
                    .Concat(Flatten(value?.Gastos))
                    .Cast<ModelBase>().ToList());
                _vm?.ClearSearch();
            }
        }

        private static IEnumerable<SubCuenta> Flatten(Cuenta? c)
        {
            return c?.Children.SelectMany(Flatten).Concat(c?.SubCuentas) ?? Array.Empty<SubCuenta>();
        }

    }
}
