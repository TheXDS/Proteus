using TheXDS.Proteus.Crud.Base;
using TheXDS.Proteus.Models;

namespace TheXDS.Proteus.ContabilidadUi.Crud
{
    /// <summary>
    /// Describe las propiedades Crud para el modelo
    /// <see cref="Banco"/>.
    /// </summary>
    public class BancoCrudDescriptor : CrudDescriptor<Banco>
    {
        /// <summary>
        /// Describe las propiedades Crud para el modelo
        /// <see cref="Banco"/>.
        /// </summary>
        protected override void DescribeModel()
        {
            OnModuleMenu(Annotations.InteractionType.Catalog);
            FriendlyName("Definición de banco");
            Property(p => p.Name).AsName();
            this.DescribeContact();
            ListProperty(p => p.Cuentas).Creatable().Required().ShowInDetails();
        }
    }

}