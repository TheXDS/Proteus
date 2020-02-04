using TheXDS.Proteus.Crud.Base;
using TheXDS.Proteus.Models;
using static TheXDS.Proteus.Annotations.InteractionType;

namespace TheXDS.Proteus.ContabilidadUi.Crud
{
    /// <summary>
    /// Describe las propiedades Crud para el modelo
    /// <see cref="AccessControlList"/>.
    /// </summary>
    public class AccessControlListDescriptor : CrudDescriptor<AccessControlList>
    {
        /// <summary>
        /// Describe las propiedades Crud para el modelo
        /// <see cref="AccessControlList"/>.
        /// </summary>
        protected override void DescribeModel()
        {
            OnModuleMenu(Settings, Modules.ContabilidadModule.IsAdmin);
            FriendlyName("Controles de acceso");

            LinkProperty<User>(p => p.UserId).Required().Important("Usuario");
            Property(p => p.EmpresaDefault).Nullable().Important("Acceso predeterminado para empresas");
            Property(p => p.EntidadDefault).Nullable().Important("Acceso predeterminado para entidades");
            Property(p => p.CostCenterDefault).Nullable().Important("Acceso predeterminado para centros de costo");

            ListProperty(p => p.Entries).Creatable().Required().Label("Accesos específicos");
        }
    }
}