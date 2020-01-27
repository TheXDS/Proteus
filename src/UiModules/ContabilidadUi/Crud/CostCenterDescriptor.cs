using TheXDS.Proteus.Crud.Base;
using TheXDS.Proteus.Models;

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
            FriendlyName("Centro de costo");
            Property(p => p.Name).AsName();
        }
    }

}