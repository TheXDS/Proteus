/*
Copyright © 2017-2019 César Andrés Morgan
Licenciado para uso interno solamente.
*/

using TheXDS.Proteus.Pages.Base;
using TheXDS.Proteus.UiDemo.ViewModels;

namespace TheXDS.Proteus.Pages
{
    /// <summary>
    /// Lógica de interacción para TestPage.xaml
    /// </summary>
    public partial class TestPage : ProteusPage
    {
        public TestPage()
        {
            InitializeComponent();
            ViewModel = new TestPageViewModel(this);
        }
    }
}
