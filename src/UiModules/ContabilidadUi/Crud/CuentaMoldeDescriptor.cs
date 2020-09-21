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
            ListProperty(p => p.Children).Creatable().Label("Cuentas hijas");
            ListProperty(p => p.SubCuentas).Creatable().Label("Auxiliares");
            ShowAllInDetails();
            CanDelete(c => c.Children.Count == 0 && c.SubCuentas.Count == 0);
        }
    }
}