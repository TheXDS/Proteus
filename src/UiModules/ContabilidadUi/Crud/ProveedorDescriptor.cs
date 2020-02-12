using TheXDS.Proteus.Crud.Base;
using TheXDS.Proteus.Models;

namespace TheXDS.Proteus.ContabilidadUi.Crud
{
    /// <summary>
    /// Describe las propiedades Crud para el modelo
    /// <see cref="Proveedor"/>.
    /// </summary>
    public class ProveedorDescriptor : CrudDescriptor<Proveedor>
    {
        /// <summary>
        /// Describe las propiedades Crud para el modelo
        /// <see cref="Proveedor"/>.
        /// </summary>
        protected override void DescribeModel()
        {
            OnModuleMenu(Annotations.InteractionType.AdminTool);
            Property(p => p.Name).AsName();
            TextProperty(p => p.Rtn).Mask("9999-9999-999999").Required().Important("RTN");
            this.DescribeContact();
            this.DescribeAddress();
            ListProperty(p => p.Empresas)
                .Creatable()
                .Required()
                .Label("Cuentas contables por empresa")
                .ShowInDetails();
        }
    }
}