using System.Windows;
using System.Windows.Controls;
using TheXDS.Proteus.ViewModels;

namespace TheXDS.Proteus.Views;
/// <summary>
/// Business logic for LoginView.xaml
/// </summary>
public partial class LoginView : UserControl
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LoginView"/> class.
    /// </summary>
    public LoginView()
    {
        InitializeComponent();
    }

    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (DataContext is LoginViewModel vm && sender is PasswordBox { Password: { } pw }) vm.Password = pw;
    }
}
