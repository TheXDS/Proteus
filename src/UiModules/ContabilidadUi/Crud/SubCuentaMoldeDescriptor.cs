using TheXDS.Proteus.Crud.Base;
using TheXDS.Proteus.Models;
using TheXDS.Proteus.ViewModels;

namespace TheXDS.Proteus.ContabilidadUi.Crud
{
    /// <summary>
    /// Describe las propiedades Crud para el modelo
    /// <see cref="SubCuentaMolde"/>.
    /// </summary>
    public class SubCuentaMoldeDescriptor : CrudDescriptor<SubCuentaMolde>
    {
        protected override void DescribeModel()
        {
            Property(p => p.Name).AsName();
            ObjectProperty(p => p.DefaultDivisa).Selectable().Nullable().Important("Divisa de la cuenta");
        }
    }


    /// <summary>
    /// Describe las propiedades Crud para el modelo
    /// <see cref="InventarioKind"/>.
    /// </summary>
    public class InventarioKindCrudDescriptor : CrudDescriptor<InventarioKind>
    {
        /// <summary>
        /// Describe las propiedades Crud para el modelo
        /// <see cref="InventarioKind"/>.
        /// </summary>
        protected override void DescribeModel()
        {
            FriendlyName("Categoría de inventario fijo");
            OnModuleMenu(Annotations.InteractionType.Settings);
            Property(p => p.Name).AsName();
            ObjectProperty(p => p.Depreciacion)
                .Creatable()
                .NotNull()
                .Required()
                .Important("Depreciación a aplicar");

            Property(p => p.LifeUnit);
            NumericProperty(p => p.LifeValue).Positive();
            ListProperty(p => p.RegistroCuentas)
                .NotEmpty().Creatable();
        }
    }


    /// <summary>
    /// Describe las propiedades Crud para el modelo
    /// <see cref="InventarioKindXEmpresa"/>.
    /// </summary>
    public class InventarioKindXEmpresaCrudDescriptor : CrudDescriptor<InventarioKindXEmpresa, InventarioKindXEmpresaViewModel>
    {
        /// <summary>
        /// Describe las propiedades Crud para el modelo
        /// <see cref="InventarioKindXEmpresa"/>.
        /// </summary>
        protected override void DescribeModel()
        {

        }
    }

}