using TheXDS.MCART.ViewModel;
using TheXDS.Proteus.Models;

namespace TheXDS.Proteus.ContabilidadUi.ViewModels
{
    /// <summary>
    /// Clase base personalizada para el ViewModel recompilado que se utilizará
    /// dentro del Crud generado para el modelo
    /// <see cref="CtaXCobrar"/>.
    /// </summary>
    public class CtaXCobrarViewModel : ViewModel<CtaXCobrar>
    {
        private DocumentKind _kind;

        /// <summary>
        ///     Obtiene o establece el valor Kind.
        /// </summary>
        /// <value>El valor de Kind.</value>
        public DocumentKind Kind
        {
            get => _kind;
            set => Change(ref _kind, value);
        }
    }
}