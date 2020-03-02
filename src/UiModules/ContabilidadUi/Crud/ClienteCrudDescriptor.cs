using TheXDS.Proteus.Crud.Base;
using TheXDS.Proteus.Models;

namespace TheXDS.Proteus.ContabilidadUi.Crud
{
    /// <summary>
    /// Describe las propiedades Crud para el modelo
    /// <see cref="Cliente"/>.
    /// </summary>
    public class ClienteCrudDescriptor : CrudDescriptor<Cliente>
    {
        /// <summary>
        /// Describe las propiedades Crud para el modelo
        /// <see cref="Cliente"/>.
        /// </summary>
        protected override void DescribeModel()
        {
            OnModuleMenu(Annotations.InteractionType.AdminTool);
            Property(p => p.Name).AsName();
            TextProperty(p => p.Rtn).Mask("9999-9999-999999").Nullable().Important("RTN");
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