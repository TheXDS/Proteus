using TheXDS.MCART.ViewModel;
using TheXDS.Proteus.Crud.Base;
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


    /// <summary>
    /// Clase base personalizada para el ViewModel recompilado que se utilizará
    /// dentro del Crud generado para el modelo
    /// <see cref="CtaXPagar"/>.
    /// </summary>
    public class CtaXPagarViewModel : ViewModel<CtaXPagar>
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

        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// <see cref="CtaXPagarViewModel"/>.
        /// </summary>
        public CtaXPagarViewModel()
        {
        }
    }
}