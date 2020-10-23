using TheXDS.Proteus.Crud.Base;
using TheXDS.Proteus.FacturacionUi.ViewModels;
using TheXDS.Proteus.Models;
using TheXDS.Proteus.Models.Base;
using TheXDS.Proteus.Models.Misc;

namespace TheXDS.Proteus.FacturacionUi.Crud
{
    /// <summary>
    /// Describe las propiedades Crud para el modelo
    /// <see cref="AutomationItem"/>.
    /// </summary>
    public class AutomationItemCrudDescriptor : CrudDescriptor<AutomationItem, AutomationItemCrudViewModel>
    {
        /// <summary>
        /// Describe las propiedades Crud para el modelo
        /// <see cref="AutomationItem"/>.
        /// </summary>
        protected override void DescribeModel()
        {
            FriendlyName("Automatización");
            VmObjectProperty(p => p.SelectedAutomationItem)
                .Selectable()
                .Source(AutomationItemSource.AutomationItems)
                .Required()
                .Label("Herramienta de automatización")
                .AsListColumn()
                .ShowInDetails();

            VmBeforeSave(SetAutomationItem);
        }

        private void SetAutomationItem(AutomationItemCrudViewModel arg1, ModelBase arg2)
        {
            if (arg1.SelectedAutomationItem?.AutomationItemGuid is { } p)
                arg1.Entity.Automator = p;
        }
    }
}