using TheXDS.Proteus.Crud.Base;
using TheXDS.Proteus.Models;

namespace TheXDS.Proteus.ContabilidadUi.Crud
{
    /// <summary>
    /// Describe las propiedades Crud para el modelo
    /// <see cref="AclEntry"/>.
    /// </summary>
    public class AclEntryDescriptor : CrudDescriptor<AclEntry>
    {
        /// <summary>
        /// Describe las propiedades Crud para el modelo
        /// <see cref="AclEntry"/>.
        /// </summary>
        protected override void DescribeModel()
        {
            FriendlyName("Control de acceso");

            ObjectProperty(p => p.Empresa).Selectable().RadioSelectable().Label("Empresa");
            ObjectProperty(p => p.Entidad).Selectable().RadioSelectable().Label("Entidad");
            ObjectProperty(p => p.CostCenter).Selectable().RadioSelectable().Label("Centro de costos");

            Property(p => p.Value).Required().Important("Acceso").Icon("🔑");
        }
    }

}