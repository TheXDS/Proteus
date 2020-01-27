using TheXDS.Proteus.Crud.Base;
using TheXDS.Proteus.Models;
using static TheXDS.Proteus.Annotations.InteractionType;

namespace TheXDS.Proteus.ContabilidadUi.Crud
{
    /// <summary>
    /// Describe las propiedades Crud para el modelo
    /// <see cref="Entidad"/>.
    /// </summary>
    public class EntidadDescriptor : CrudDescriptor<Entidad>
    {
        /// <summary>
        /// Describe las propiedades Crud para el modelo
        /// <see cref="Entidad"/>.
        /// </summary>
        protected override void DescribeModel()
        {
            OnModuleMenu(AdminTool);
            Property(p => p.Name).AsName();
        }
    }

}