/*
Copyright © 2017-2020 César Andrés Morgan
Licenciado para uso interno solamente.
*/

using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TheXDS.MCART.Component;
using TheXDS.MCART.Dialogs;
using TheXDS.MCART.Pages;
using TheXDS.Proteus.Resources;

namespace TheXDS.Proteus.DevelModule.Pages
{
    /// <summary>
    /// Lógica de interacción para ComponentOverviewPage.xaml
    /// </summary>
    public partial class ComponentOverviewPage : Page
    {
        public ComponentOverviewPage()
        {
            InitializeComponent();
        }

        private void LstPlugins_DblClick(object sender, MouseButtonEventArgs e)
        {
            var t = ((sender as FrameworkElement)?.DataContext as IExposeAssembly)?.Assembly;
            if (t is null) return;
            var i = new ApplicationInfo(t, InferImage(t));
            AboutBox.ShowDialog(i);
        }

        private static UIElement InferImage(Assembly t)
        {
            if (t == typeof(Proteus).Assembly) return Images.Proteus;
            if (t == typeof(App).Assembly) return Images.Logo;
            return Images.Plugin;
        }


        private void BtnTypeInfo_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement fe)
            {
                new Window
                {
                    Content = new TypeDetails(fe.DataContext?.GetType())
                }.ShowDialog();
            }
        }
    }
}
