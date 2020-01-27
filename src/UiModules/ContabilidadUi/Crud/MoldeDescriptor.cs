using TheXDS.Proteus.Crud.Base;
using TheXDS.Proteus.Models;
using static TheXDS.Proteus.Annotations.InteractionType;

namespace TheXDS.Proteus.ContabilidadUi.Crud
{
    /// <summary>
    /// Describe las propiedades Crud para el modelo
    /// <see cref="Molde"/>.
    /// </summary>
    public class MoldeDescriptor : CrudDescriptor<Molde>
    {
        protected override void DescribeModel()
        {
            OnModuleMenu(Settings);
            FriendlyName("Molde de árbol contable");

            Property(p => p.Name).AsName().AsListColumn();
            ObjectProperty(p => p.Activo).Creatable().Important("Molde de cuenta de activo").Required();
            ObjectProperty(p => p.Pasivo).Creatable().Important("Molde de cuenta de pasivo").Required();
            ObjectProperty(p => p.Patrimonio).Creatable().Important("Molde de cuenta de capital").Required();

            ShowAllInDetails();
        }
    }
}