using System.ComponentModel;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Windows.Data;
using TheXDS.MCART.Types.Extensions;
using TheXDS.MCART.ViewModel;
using TheXDS.Proteus.Component;
using TheXDS.Proteus.Misc;
using TheXDS.Proteus.Models;
using TheXDS.Proteus.ViewModels.Base;

namespace TheXDS.Proteus.ContabilidadUi.ViewModels
{
    /// <summary>
    /// Implementa la funcionalidad de ingreso de partidas normales en el
    /// sistema.
    /// </summary>
    public class PartidaIngresoViewModel : PageViewModel, IEntityViewModel<Partida>
    {
        private Partida _entity;
        private ICollectionView _cuentaResults;
        private bool _cuentaSelectMode;
        private bool _isSearchingCuenta;
        private string? _cuentaSearchQuery;
        private SubCuenta? _cuentaSelection;
        private bool _willCuentaSearch;
        private bool _costCtrSelectMode;
        private bool _isSearchingCostCtr;
        private string? _costCtrSearchQuery;
        private CostCenter? _costCtrSelection;
        private bool _willCostCtrSearch;
        private decimal _cuentaValue;
        private ICollectionView _costCtrResults;

        /// <summary>
        ///     Obtiene o establece el valor CuentaSelection.
        /// </summary>
        /// <value>El valor de CuentaSelection.</value>
        public SubCuenta? CuentaSelection
        {
            get => _cuentaSelection;
            set => Change(ref _cuentaSelection, value);
        }

        /// <summary>
        /// Obtiene un valor que indica si al ejecutar
        /// <see cref="CuentaSearchCommand"/> se hará una búsqueda o se limpiará
        /// la búsqueda actual.
        /// </summary>
        public bool WillCuentaSearch
        {
            get => _willCuentaSearch;
            private set => Change(ref _willCuentaSearch, value);
        }

        /// <summary>
        ///     Obtiene o establece el valor CuentaSearchQuery.
        /// </summary>
        /// <value>El valor de CuentaSearchQuery.</value>
        public string? CuentaSearchQuery
        {
            get => _cuentaSearchQuery;
            set => Change(ref _cuentaSearchQuery, value);
        }

        /// <summary>
        ///     Obtiene o establece el valor IsSearchingCuenta.
        /// </summary>
        /// <value>El valor de IsSearchingCuenta.</value>
        public bool IsSearchingCuenta
        {
            get => _isSearchingCuenta;
            set => Change(ref _isSearchingCuenta, value);
        }

        /// <summary>
        ///     Obtiene o establece el valor CuentaSelectMode.
        /// </summary>
        /// <value>El valor de CuentaSelectMode.</value>
        public bool CuentaSelectMode
        {
            get => _cuentaSelectMode;
            set => Change(ref _cuentaSelectMode, value);
        }


        /// <summary>
        ///     Obtiene o establece el valor CostCtrSelection.
        /// </summary>
        /// <value>El valor de CostCtrSelection.</value>
        public CostCenter? CostCtrSelection
        {
            get => _costCtrSelection;
            set => Change(ref _costCtrSelection, value);
        }

        /// <summary>
        /// Obtiene un valor que indica si al ejecutar
        /// <see cref="CostCtrSearchCommand"/> se hará una búsqueda o se limpiará
        /// la búsqueda actual.
        /// </summary>
        public bool WillCostCtrSearch
        {
            get => _willCostCtrSearch;
            private set => Change(ref _willCostCtrSearch, value);
        }

        /// <summary>
        ///     Obtiene o establece el valor CostCtrSearchQuery.
        /// </summary>
        /// <value>El valor de CostCtrSearchQuery.</value>
        public string? CostCtrSearchQuery
        {
            get => _costCtrSearchQuery;
            set => Change(ref _costCtrSearchQuery, value);
        }

        /// <summary>
        ///     Obtiene o establece el valor IsSearchingCostCtr.
        /// </summary>
        /// <value>El valor de IsSearchingCostCtr.</value>
        public bool IsSearchingCostCtr
        {
            get => _isSearchingCostCtr;
            set => Change(ref _isSearchingCostCtr, value);
        }

        /// <summary>
        ///     Obtiene o establece el valor CostCtrSelectMode.
        /// </summary>
        /// <value>El valor de CostCtrSelectMode.</value>
        public bool CostCtrSelectMode
        {
            get => _costCtrSelectMode;
            set => Change(ref _costCtrSelectMode, value);
        }



        /// <summary>
        ///     Obtiene o establece el valor Entity.
        /// </summary>
        /// <value>El valor de Entity.</value>
        public Partida Entity
        {
            get => _entity;
            set => Change(ref _entity, value);
        }

        /// <summary>
        ///     Obtiene o establece el valor CuentaResults.
        /// </summary>
        /// <value>El valor de CuentaResults.</value>
        public ICollectionView CuentaResults
        {
            get => _cuentaResults;
            set => Change(ref _cuentaResults, value);
        }
        
        /// <summary>
        ///     Obtiene o establece el valor CuentaResults.
        /// </summary>
        /// <value>El valor de CuentaResults.</value>
        public ICollectionView CostCtrResults
        {
            get => _costCtrResults;
            set => Change(ref _costCtrResults, value);
        }

        /// <summary>
        ///     Obtiene o establece el valor CuentaValue.
        /// </summary>
        /// <value>El valor de CuentaValue.</value>
        public decimal CuentaValue
        {
            get => _cuentaValue;
            set => Change(ref _cuentaValue, value);
        }



        /// <summary>
        /// Obtiene la etiqueta a utilizar para mostrar sobre el botón de
        /// búsqueda.
        /// </summary>
        public string CuentaSearchLabel => WillCuentaSearch ? "🔍" : "❌";

        /// <summary>
        /// Obtiene el comando relacionado a la acción Search.
        /// </summary>
        /// <returns>El comando Search.</returns>
        public ObservingCommand CuentaSearchCommand { get; }

        /// <summary>
        ///     Obtiene el comando relacionado a la acción CancelSelectCuenta.
        /// </summary>
        /// <returns>El comando CancelSelectCuenta.</returns>
        public SimpleCommand CancelSelectCuentaCommand { get; }

        /// <summary>
        /// Obtiene el comando relacionado a la acción OkSelectCuenta.
        /// </summary>
        /// <returns>El comando OkSelectCuenta.</returns>
        public ObservingCommand OkSelectCuentaCommand { get; }

        /// <summary>
        /// Obtiene el comando relacionado a la acción Search.
        /// </summary>
        /// <returns>El comando Search.</returns>
        public ObservingCommand CostCtrSearchCommand { get; }

        /// <summary>
        ///     Obtiene el comando relacionado a la acción CancelSelectCostCtr.
        /// </summary>
        /// <returns>El comando CancelSelectCostCtr.</returns>
        public SimpleCommand CancelSelectCostCtrCommand { get; }

        /// <summary>
        /// Obtiene el comando relacionado a la acción OkSelectCostCtr.
        /// </summary>
        /// <returns>El comando OkSelectCostCtr.</returns>
        public ObservingCommand OkSelectCostCtrCommand { get; }


        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// <see cref="PartidaIngresoViewModel"/>.
        /// </summary>
        /// <param name="host">Página Host de este ViewModel.</param>
        public PartidaIngresoViewModel(ICloseable host) : base(host)
        {            
            _entity = new Partida();

            RegisterPropertyChangeBroadcast(nameof(WillCuentaSearch), nameof(CuentaSearchLabel));

            CuentaSearchCommand = new ObservingCommand(this, OnSearchCuenta)
                .ListensToProperty(() => CuentaSearchQuery!)
                .SetCanExecute(() => !CuentaSearchQuery.IsEmpty());

            OkSelectCuentaCommand = new ObservingCommand(this, OnOkCuentaSelect)
                .ListensToProperty(() => CuentaSelection)
                .SetCanExecute(() => CuentaSelection != null);

            CancelSelectCuentaCommand = new SimpleCommand(OnCancelSelectCuenta);

            CostCtrSearchCommand = new ObservingCommand(this, OnSearchCostCtr)
                .ListensToProperty(() => CostCtrSearchQuery!)
                .SetCanExecute(() => !CostCtrSearchQuery.IsEmpty());

            OkSelectCostCtrCommand = new ObservingCommand(this, OnOkCostCtrSelect)
                .ListensToProperty(() => CostCtrSelection)
                .SetCanExecute(() => CostCtrSelection != null);

            CancelSelectCostCtrCommand = new SimpleCommand(OnCancelSelectCostCtr);

        }




        private void OnCancelSelectCuenta()
        {
            CuentaSelection = null;
            OnOkCuentaSelect();
        }

        private void OnOkCuentaSelect()
        {
            CuentaSelectMode = false;
            IsBusy = false;
            ClearCuentaSearch();
        }

        /// <summary>
        /// Limpia los resultados de la búsqueda.
        /// </summary>
        public void ClearCuentaSearch()
        {
            CuentaResults = null;
            CuentaSearchQuery = null;
        }

        private async void OnSearchCuenta()
        {
            CuentaSelection = null;
            if (WillCuentaSearch && !CuentaSearchQuery.IsEmpty()) await PerformCuentaSearch();
            else ClearCuentaSearch();
        }

        private async Task PerformCuentaSearch()
        {
            IsSearchingCuenta = true;

            CuentaResults = CollectionViewSource.GetDefaultView(await Internal.Query(CuentaSearchQuery!,typeof(SubCuenta)).ToListAsync());
            CuentaResults.Refresh();

            IsSearchingCuenta = false;
            WillCuentaSearch = false;
        }

        private void OnCancelSelectCostCtr()
        {
            CostCtrSelection = null;
            OnOkCostCtrSelect();
        }

        private void OnOkCostCtrSelect()
        {
            CostCtrSelectMode = false;
            IsBusy = false;
            ClearCostCtrSearch();
        }

        /// <summary>
        /// Limpia los resultados de la búsqueda.
        /// </summary>
        public void ClearCostCtrSearch()
        {
            CostCtrResults = null;
            CostCtrSearchQuery = null;
        }

        private async void OnSearchCostCtr()
        {
            CostCtrSelection = null;
            if (WillCostCtrSearch && !CostCtrSearchQuery.IsEmpty()) await PerformCostCtrSearch();
            else ClearCostCtrSearch();
        }

        private async Task PerformCostCtrSearch()
        {
            IsSearchingCostCtr = true;

            CostCtrResults = CollectionViewSource.GetDefaultView(await Internal.Query(CostCtrSearchQuery!, typeof(CostCenter)).ToListAsync());
            CostCtrResults.Refresh();

            IsSearchingCostCtr = false;
            WillCostCtrSearch = false;
        }

    }
}