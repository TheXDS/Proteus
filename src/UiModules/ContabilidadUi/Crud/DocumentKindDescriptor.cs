using TheXDS.Proteus.Crud.Base;
using TheXDS.Proteus.Models;
using static TheXDS.Proteus.Annotations.InteractionType;

namespace TheXDS.Proteus.ContabilidadUi.Crud
{
    /// <summary>
    /// Describe las propiedades Crud para el modelo
    /// <see cref="DocumentKind"/>.
    /// </summary>
    public class DocumentKindDescriptor : CrudDescriptor<DocumentKind>
    {
        /// <summary>
        /// Describe las propiedades Crud para el modelo
        /// <see cref="DocumentKind"/>.
        /// </summary>
        protected override void DescribeModel()
        {
            OnModuleMenu(Settings);
            FriendlyName("Tipo de documento");

            Property(p => p.Prefix)
                .Important("Prefijo")
                .ShowInDetails()
                .AsListColumn()
                .Default((byte)1);

            Property(p => p.Name).AsName();
        }
    }
}