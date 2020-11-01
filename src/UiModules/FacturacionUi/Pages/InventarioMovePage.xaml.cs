using TheXDS.Proteus.FacturacionUi.ViewModels;
using TheXDS.Proteus.Pages.Base;

namespace TheXDS.Proteus.FacturacionUi.Pages
{
    /// <summary>
    /// Lógica de interacción para InventarioMovePage.xaml
    /// </summary>
    public partial class InventarioMovePage : ProteusPage
    {
        public InventarioMovePage()
        {
            InitializeComponent();
            Loaded += InventarioMovePage_Loaded;
        }

        private async void InventarioMovePage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            DataContext = new InventarioMoveViewModel(this);
        }
    }
}
