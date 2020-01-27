using TheXDS.Proteus.Crud.Base;
using TheXDS.Proteus.Models;
using static TheXDS.Proteus.Annotations.InteractionType;

namespace TheXDS.Proteus.ContabilidadUi.Crud
{
    /// <summary>
    /// Describe las propiedades Crud para el modelo
    /// <see cref="Divisa"/>.
    /// </summary>
    public class DivisaDescriptor : CrudDescriptor<Divisa>
    {
        /// <summary>
        /// Describe las propiedades Crud para el modelo
        /// <see cref="Divisa"/>.
        /// </summary>
        protected override void DescribeModel()
        {
            OnModuleMenu(Settings);

            Property(p => p.Id).Id("Código ISO 3166 regional");
            Property(p => p.Name).AsName();
            Property(p => p.Cuentas).OnlyInDetails();
        }
    }
}