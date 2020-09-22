using TheXDS.Proteus.Annotations;
using TheXDS.Proteus.Crud.Base;
using TheXDS.Proteus.Models;

namespace TheXDS.Proteus.FacturacionUi.Crud
{
    /// <summary>
    /// Describe las propiedades Crud para el modelo
    /// <see cref="GiftCard"/>.
    /// </summary>
    public class GiftCardCrudDescriptor : CrudDescriptor<GiftCard>
    {
        /// <summary>
        /// Describe las propiedades Crud para el modelo
        /// <see cref="GiftCard"/>.
        /// </summary>
        protected override void DescribeModel()
        {
            CanCreate(false);
            CanEdit(false);          
            OnModuleMenu(InteractionType.AdminTool);
            FriendlyName("Tarjeta de regalo");
            Property(p => p.Id).Id("Id de tarjeta").ReadOnly();
            Property(p => p.Timestamp).OnlyInDetails();
            Property(p => p.Amount).OnlyInDetails();
            Property(p => p.Void).OnlyInDetails();
            AllListColumn();
            ShowAllInDetails();
        }
    }
}