using System.Windows.Controls;
using TheXDS.Proteus.Api;
using TheXDS.Proteus.ViewModels;

namespace TheXDS.Proteus.ContabilidadUi.Pages
{
    /// <summary>
    /// Lógica de interacción para ContabMainMenuPage.xaml
    /// </summary>
    public partial class ContabMainMenuPage : UserControl
    {
        public ContabMainMenuPage()
        {
            InitializeComponent();
            DataContext = ContabilidadService.ModuleStatus;
        }
    }
}
