using TheXDS.Proteus.Crud.Base;
using TheXDS.Proteus.Models;

namespace TheXDS.Proteus.ContabilidadUi.Crud
{
    /// <summary>
    /// Describe las propiedades Crud para el modelo
    /// <see cref="DocumentRef"/>.
    /// </summary>
    public class DocumentRefDescriptor : CrudDescriptor<DocumentRef>
    {
        /// <summary>
        /// Describe las propiedades Crud para el modelo
        /// <see cref="DocumentRef"/>.
        /// </summary>
        protected override void DescribeModel()
        {
            ObjectProperty(p => p.Kind).Important("Tipo de documento").Required();
            Property(p => p.DocReference).Important("Número de referencia").NotEmpty();
            TextProperty(p => p.FilePath)
                .TextKind(TextKind.FilePath)
                .Nullable()
                .Label("Archivo de referencia");
        }
    }
}