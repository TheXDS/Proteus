/*
Copyright © 2017-2020 César Andrés Morgan
Licenciado para uso interno solamente.
*/

using System;
using System.Windows;
using TheXDS.Proteus.Api;
using TheXDS.Proteus.Component;
using TheXDS.Proteus.FacturacionUi.Lib;

namespace TheXDS.Proteus.Dialogs
{
    /// <summary>
    ///     Lógica de interacción para Fact2ndScreen.xaml
    /// </summary>
    public partial class Fact2ndScreen : ICloseable
    {
        public Fact2ndScreen()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.Manual;
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.ToScreen(FacturaService.GetEstation!.SecondScreen ?? 1);
            }
            catch (Exception ex)
            {
                Proteus.AlertTarget?.Alert("Hubo un problema al abrir la pantalla secundaria.", ex.Message);
                Close();
            }
        }
    }
}
