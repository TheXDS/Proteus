using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using TheXDS.MCART.Controls;
using TheXDS.MCART.Types;
using TheXDS.MCART.Types.Base;
using TheXDS.MCART.ViewModel;
using TheXDS.Proteus.Api;
using TheXDS.Proteus.ContabilidadUi.Modules;
using TheXDS.Proteus.Models;
using TheXDS.Proteus.Models.Base;
using TheXDS.Proteus.ViewModels.Base;

namespace TheXDS.Proteus.ViewModels
{
    /// <summary>
    /// Clase base personalizada para el ViewModel recompilado que se utilizará
    /// dentro del Crud generado para el modelo
    /// <see cref="ProveedorXEmpresa"/>.
    /// </summary>
    public class ProveedorXEmpresaViewModel : ViewModel<ProveedorXEmpresa>
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// <see cref="ProveedorXEmpresaViewModel"/>.
        /// </summary>
        public ProveedorXEmpresaViewModel()
        {
        }

        public ObservableListWrap<ModelBase> CurrentSubCuentas { get; } = new ObservableListWrap<ModelBase>();


        private Empresa _selectedEmpresa;

        /// <summary>
        ///     Obtiene o establece el valor SelectedEmpresa.
        /// </summary>
        /// <value>El valor de SelectedEmpresa.</value>
        public Empresa SelectedEmpresa
        {
            get => _selectedEmpresa;
            set
            {
                if (!Change(ref _selectedEmpresa, value)) return;
                Entity.Empresa = value;
                CurrentSubCuentas
                    .Substitute(Flatten(value.Activo)
                    .Concat(Flatten(value.Pasivo))
                    .Concat(Flatten(value.Patrimonio))
                    .Cast<ModelBase>().ToList());
            }
        }

        private static IEnumerable<SubCuenta> Flatten(Cuenta c)
        {
            return c.Children.SelectMany(Flatten).Concat(c.SubCuentas);
        }
    }

    public class ContabManagerViewModel : ProteusViewModel, IAsyncRefreshable
    {
        private Empresa? _activeEmpresa;
        private Entidad? _activeEntidad;
        private Periodo? _activePeriodo;
        private bool _canSelectEmpresa;
        private bool _canSelectEntidad;
        private bool _canSelectPeriodo;

        private IEnumerable<Empresa>? _allEmpresas;

        /// <summary>
        /// Enumera todas las empresas del sistema.
        /// </summary>
        public IEnumerable<Empresa>? AllEmpresas
        {
            get => _allEmpresas;
            private set => Change(ref _allEmpresas, value);
        }

        /// <summary>
        /// Obtiene o establece el valor ActiveEmpresa.
        /// </summary>
        /// <value>El valor de ActiveEmpresa.</value>
        public Empresa? ActiveEmpresa
        {
            get => _activeEmpresa;
            set
            {
                if (Change(ref _activeEmpresa, value)) CrunchData();
            }
        }

        /// <summary>
        ///     Obtiene o establece el valor ActiveEntidad.
        /// </summary>
        /// <value>El valor de ActiveEntidad.</value>
        public Entidad? ActiveEntidad
        {
            get => _activeEntidad;
            set => Change(ref _activeEntidad, value);
        }

        /// <summary>
        /// Obtiene o establece el valor ActivePeriodo.
        /// </summary>
        /// <value>El valor de ActivePeriodo.</value>
        public Periodo? ActivePeriodo
        {
            get => _activePeriodo;
            set
            {
                if (!Change(ref _activePeriodo, value) || value is null) return;
                UpdateGraph();
            }
        }

        /// <summary>
        ///     Obtiene o establece el valor canSelectEmpresa.
        /// </summary>
        /// <value>El valor de canSelectEmpresa.</value>
        public bool CanSelectEmpresa
        {
            get => _canSelectEmpresa;
            private set => Change(ref _canSelectEmpresa, value);
        }

        /// <summary>
        ///     Obtiene o establece el valor CanSelectEntidad.
        /// </summary>
        /// <value>El valor de CanSelectEntidad.</value>
        public bool CanSelectEntidad
        {
            get => _canSelectEntidad;
            private set => Change(ref _canSelectEntidad, value);
        }

        /// <summary>
        ///     Obtiene o establece el valor CanSelectPeriodo.
        /// </summary>
        /// <value>El valor de CanSelectPeriodo.</value>
        public bool CanSelectPeriodo
        {
            get => _canSelectPeriodo;
            private set => Change(ref _canSelectPeriodo, value);
        }

        public LightGraph Graph { get; }

        public ContabManagerViewModel()
        {
            Graph = new LightGraph
            {
                SpotLabels = SpotLabelsDrawMode.YValues,
                Margin = new System.Windows.Thickness(20, 50, 20, 20),
                Height = 500,
                GraphDrawMode = EnumGraphDrawMode.Histogram,
                GraphTitle = "Movimientos en cuentas (úlitmas 10 partidas)",
                XLabel = "Cuenta",
                YLabel = "Debe",
                Y2Label = "Haber",
                GraphStroke = Brushes.Green,
                Graph2Stroke = Brushes.Red
            };
        }

        private async Task UpdateGraph()
        {
            IsBusy = Graph.Frozen = true;
            Graph.Graph.Clear();
            Graph.Graph2.Clear();
            Graph.XLabels.Clear();

            var m = await Task.Run(() => ContabilidadModule.ModuleStatus.ActivePeriodo!.Partidas.TakeLast(10).SelectMany(p => p.Movimientos).GroupBy(p => p.Cuenta));
            if (m.Any())
            {
                Graph.Y2Max = Graph.YMax = (double)m.SelectMany(p => p).Max(p => p.RawValue) * 1.1;
                Graph.Y2Min = 0.0;// Graph.YMin = (double)m.SelectMany(p => p).Min(p => p.RawValue) * 1.1;
                foreach (var j in m)
                {
                    Graph.XLabels.Add(j.Key.ToString());
                    Graph.Graph.Add((double)j.Sum(p => p.RawValue > 0m ? p.RawValue : 0m));
                    Graph.Graph2.Add((double)j.Sum(p => p.RawValue < 0m ? -p.RawValue : 0m));
                }
                GraphVisible = Visibility.Visible;
            }
            else
            { 
                GraphVisible = Visibility.Collapsed;
            }
            IsBusy = Graph.Frozen = false;
            Graph.Redraw();
        }


        private Visibility _graphVisible;

        /// <summary>
        ///     Obtiene o establece el valor GraphVisible.
        /// </summary>
        /// <value>El valor de GraphVisible.</value>
        public Visibility GraphVisible
        {
            get => _graphVisible;
            set => Change(ref _graphVisible, value);
        }



        public IQueryable<Entidad> ActiveEntidades()
        {
            return ActiveEmpresa?.Entidades.AsQueryable() ?? Array.Empty<Entidad>().AsQueryable();
        }
        public IQueryable<SubCuenta> ActiveSubCuentas()
        {
            if (ActiveEmpresa is null) return Array.Empty<SubCuenta>().AsQueryable();
            return WalkCuenta(ActiveEmpresa.Activo)
                .Concat(WalkCuenta(ActiveEmpresa.Pasivo))
                .Concat(WalkCuenta(ActiveEmpresa.Patrimonio)).AsQueryable();
        }


        public IQueryable<CostCenter>? ActiveCostCenters()
        {
            return ActiveEntidad?.CostCenters.AsQueryable() ?? Array.Empty<CostCenter>().AsQueryable();
        }

        private static List<SubCuenta> WalkCuenta(Cuenta c)
        {
            var r = new List<SubCuenta>();
            foreach (var j in c.Children)
            {
                r.AddRange(WalkCuenta(j));
            }
            r.AddRange(c.SubCuentas);
            return r;
        }
        private async Task CrunchData()
        {
            IsBusy = true;
            ActivePeriodo = ActiveEmpresa?.Periodos.FirstOrDefault(p => p.Void == null) ?? ActiveEmpresa?.Periodos.LastOrDefault();
            if (ActiveEmpresa?.Entidades.Count <= 1)
            {
                ActiveEntidad = ActiveEmpresa?.Entidades.SingleOrDefault();
                CanSelectEntidad = false;
            }
            else
            {
                CanSelectEntidad = true;
            }

            IsBusy = false;
        }

        /// <summary>
        /// Inicializa el ViewModel de estado para el servicio de contabilidad.
        /// </summary>
        /// <returns></returns>
        public async Task InitViewModel()
        {
            IsBusy = true;
            var _instance = Proteus.Service<ContabilidadService>();
            AllEmpresas = await (_instance?.All<Empresa>().ToListAsync() ?? Task.FromResult(new List<Empresa>()));
            if (AllEmpresas.Count() <= 1) 
            {
                ActiveEmpresa = AllEmpresas.SingleOrDefault();
                CanSelectEmpresa = false;
            }
            else
            {
                CanSelectEmpresa = true;
            }

            IsBusy = false;
        }

        /// <inheritdoc/>
        public Task RefreshAsync()
        {
            //await CrunchData();
            return UpdateGraph();
        }
    }
}