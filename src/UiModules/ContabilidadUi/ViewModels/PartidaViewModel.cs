using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
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
    /// Clase base personalizada para el ViewModel recompilado que se utilizará
    /// dentro del Crud generado para el modelo
    /// <see cref="Partida"/>.
    /// </summary>
    public class PartidaViewModel : ViewModel<Partida>
    {
        /// <summary>
        /// Obtiene el valor de cuadre de la partida.
        /// </summary>
        public decimal Cuadre => Entity is { } e && e.Movimientos.Any() ? e.Movimientos.Sum(p => p.RawValue) : 0m;


        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// <see cref="PartidaViewModel"/>.
        /// </summary>
        public PartidaViewModel()
        {
            RegisterPropertyChangeBroadcast(nameof(Partida.Movimientos), nameof(Cuadre));
        }
    }

    /// <summary>
    /// Implementa la funcionalidad de ingreso de partidas normales en el
    /// sistema.
    /// </summary>
    public class PartidaIngresoViewModel : PageViewModel, IEntityViewModel<Partida>
    {
        private Partida _entity;
        private ICollectionView _CuentaResults;
        private bool _cuentaSelectMode;
        private bool _isSearchingCuenta;
        private string? _cuentaSearchQuery;
        private SubCuenta? _cuentaSelection;
        private bool _willCuentaSearch;

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
        /// Obtiene el comando relacionado a la acción Search.
        /// </summary>
        /// <returns>El comando Search.</returns>
        public ObservingCommand CuentaSearchCommand { get; }

        /// <summary>
        ///     Obtiene el comando relacionado a la acción CancelSelectCuenta.
        /// </summary>
        /// <returns>El comando CancelSelectCuenta.</returns>
        public SimpleCommand CancelSelectCuentaCommand { get; }

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

        /// <summary>
        /// Obtiene el comando relacionado a la acción OkSelectCuenta.
        /// </summary>
        /// <returns>El comando OkSelectCuenta.</returns>
        public ObservingCommand OkSelectCuentaCommand { get; }

        /// <summary>
        /// Obtiene la etiqueta a utilizar para mostrar sobre el botón de
        /// búsqueda.
        /// </summary>
        public string CuentaSearchLabel => WillCuentaSearch ? "🔍" : "❌";

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
        /// Inicializa una nueva instancia de la clase
        /// <see cref="PartidaIngresoViewModel"/>.
        /// </summary>
        /// <param name="host">Página Host de este ViewModel.</param>
        public PartidaIngresoViewModel(ICloseable host) : base(host)
        {            
            _entity = new Partida();
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
            get => _CuentaResults;
            set => Change(ref _CuentaResults, value);
        }



    }
}