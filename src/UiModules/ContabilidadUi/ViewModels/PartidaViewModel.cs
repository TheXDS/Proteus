using System.ComponentModel;
using System.Linq;
using TheXDS.MCART.ViewModel;
using TheXDS.Proteus.Component;
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