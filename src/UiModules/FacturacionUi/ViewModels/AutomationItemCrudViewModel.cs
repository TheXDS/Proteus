using System.Linq;
using TheXDS.MCART.ViewModel;
using TheXDS.Proteus.Models;
using TheXDS.Proteus.Models.Misc;

namespace TheXDS.Proteus.FacturacionUi.ViewModels
{
    public class AutomationItemCrudViewModel : ViewModel<AutomationItem>
    {
        private AutomationItemSource? _selectedAutomationItem = null!;

        /// <summary>
        /// Obtiene o establece el valor SelectedPrintDriver.
        /// </summary>
        /// <value>El valor de SelectedPrintDriver.</value>
        public AutomationItemSource SelectedAutomationItem
        {
            get => _selectedAutomationItem ?? AutomationItemSource.AutomationItems.FirstOrDefault(p => p.AutomationItemGuid == Entity.Automator);
            set => Change(ref _selectedAutomationItem, value);
        }
    }
}