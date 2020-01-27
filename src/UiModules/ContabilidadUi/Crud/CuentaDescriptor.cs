using TheXDS.Proteus.Crud.Base;
using TheXDS.Proteus.Models;

namespace TheXDS.Proteus.ContabilidadUi.Crud
{
    /// <summary>
    /// Describe las propiedades Crud para el modelo
    /// <see cref="Cuenta"/>.
    /// </summary>
    public class CuentaDescriptor : CrudDescriptor<Cuenta>
    {
        /// <summary>
        /// Describe el modelo <see cref="Cuenta"/>.
        /// </summary>
        protected override void DescribeModel()
        {
            Property(p => p.Name).AsName().AsListColumn();
            ListProperty(p => p.Children).Creatable().Important("Cuentas hijas");
            ListProperty(p => p.SubCuentas).Creatable().Important("Sub-cuentas");
            ShowAllInDetails();
            Template();

            CanDelete(c => c.Parent is { } && c.Children.Count == 0 && c.SubCuentas.Count == 0);
        }
    }
}