using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TheXDS.MCART.ViewModel;
using TheXDS.Proteus.Api;
using TheXDS.Proteus.Models;
using System.Data.Entity;
using TheXDS.Proteus.Component;
using TheXDS.Proteus.Context;
using TheXDS.Proteus.ViewModels;
using System.Linq;

namespace TheXDS.Proteus.ViewModels
{
    public class ContabManagerViewModel : ViewModelBase
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
            set => Change(ref _activePeriodo, value);
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
        private async void CrunchData()
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
    }
}