using TheXDS.MCART.ViewModel;
using TheXDS.Proteus.Models;

namespace TheXDS.Proteus.ContabilidadUi.ViewModels
{
    /// <summary>
    /// Clase base personalizada para el ViewModel recompilado que se utilizará
    /// dentro del Crud generado para el modelo
    /// <see cref="Empresa"/>.
    /// </summary>
    public class EmpresaViewModel : ViewModel<Empresa>
    {
        /// <summary>
        /// Permite seleccionar un molde opcional para generar el árbol
        /// contable.
        /// </summary>
        public Molde? FromMolde { get; set; }

        /// <summary>
        /// Obtiene un valor que indica si es posible generar el árbol contable
        /// para una nueva entidad.
        /// </summary>
        public bool CanAddMolde => Entity?.IsNew ?? false;
    }
}