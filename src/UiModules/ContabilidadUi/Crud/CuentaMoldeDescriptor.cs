using TheXDS.Proteus.Crud.Base;
using TheXDS.Proteus.Models;

namespace TheXDS.Proteus.ContabilidadUi.Crud
{
    /// <summary>
    /// Describe las propiedades Crud para el modelo
    /// <see cref="CuentaMolde"/>.
    /// </summary>
    public class CuentaMoldeDescriptor : CrudDescriptor<CuentaMolde>
    {
        protected override void DescribeModel()
        {
            Property(p => p.Name).AsName().AsListColumn();
            ListProperty(p => p.Children).Creatable().Important("Cuentas hijas");
            ShowAllInDetails();
            CanDelete(c => c.Parent is { } || c.Children.Count == 0);

        }
    }

}