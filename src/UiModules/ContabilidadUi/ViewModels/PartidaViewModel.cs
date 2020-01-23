using System.Linq;
using TheXDS.MCART.ViewModel;
using TheXDS.Proteus.Models;

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
}