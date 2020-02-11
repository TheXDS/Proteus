using TheXDS.Proteus.Crud.Base;
using TheXDS.Proteus.Models;

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
}