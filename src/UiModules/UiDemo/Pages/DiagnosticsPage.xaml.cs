using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TheXDS.Proteus.Api;

namespace TheXDS.Proteus.UiDemo.Pages
{
    /// <summary>
    /// Lógica de interacción para DiagnosticsPage.xaml
    /// </summary>
    public partial class DiagnosticsPage
    {
        public DiagnosticsPage()
        {
            InitializeComponent();
            //Title = "Información de diagnóstico de servicios";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Proteus.MessageTarget?.Info((LstServices.SelectedItem as Service)?.ChangesPending().ToString() ?? "Seleccione un servicio para continuar.");
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            await ((LstServices.SelectedItem as Service)?.ForcefullySaveAsync() ?? Task.FromResult(DetailedResult.Ok));
        }
    }
}