using TheXDS.Proteus.ContabilidadUi.Modules;
using TheXDS.Proteus.Crud.Base;
using TheXDS.Proteus.Models;
using static TheXDS.Proteus.Annotations.InteractionType;

namespace TheXDS.Proteus.ContabilidadUi.Crud
{
    /// <summary>
    /// Describe las propiedades Crud para el modelo
    /// <see cref="CostCenter"/>.
    /// </summary>
    public class CostCenterDescriptor : CrudDescriptor<CostCenter>
    {
        /// <summary>
        /// Describe las propiedades Crud para el modelo
        /// <see cref="CostCenter"/>.
        /// </summary>
        protected override void DescribeModel()
        {
            OnModuleMenu(AdminTool, ContabilidadModule.CanOpen);
            FriendlyName("Centro de costo");
            ObjectProperty(p => p.Parent)
                .Selectable()
                .Required()
                .Label("Entidad padre")
                .AsListColumn()
                .ShowInDetails();
            Property(p => p.Name).AsName();
        }
    }
}