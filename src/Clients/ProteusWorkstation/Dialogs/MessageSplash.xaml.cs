/*
Copyright © 2017-2020 César Andrés Morgan
Licenciado para uso interno solamente.
*/

using TheXDS.Proteus.Component;
using SourceChord.FluentWPF;
using System.Windows;

namespace TheXDS.Proteus.Dialogs
{
    /// <summary>
    /// Interaction logic for MessageSplash.xaml
    /// </summary>
    public partial class MessageSplash : AcrylicWindow, ICloseable
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// <see cref="MessageSplash"/>.
        /// </summary>
        public MessageSplash()
        {
            InitializeComponent();
            Loaded += MessageSplash_Loaded;
        }

        private void MessageSplash_Loaded(object sender, RoutedEventArgs e)
        {
            BtnClose.Focus();
        }

        /// <summary>
        /// Muestra una mensaje por medio de una ventana
        /// <see cref="MessageSplash"/>.
        /// </summary>
        /// <param name="title">Título del mensaje.</param>
        /// <param name="message">Mensaje a mostrar.</param>
        /// <param name="type">
        /// Tipo de mensaje. Afecta la apariencia de la ventana generada.
        /// </param>
        public static void Show(string title, string message, MessageType type)
        {
            App.UiInvoke(() => {
                var m = new MessageSplash();
                m.DataContext = new MessageSplashViewModel(m, title, message, type);
                m.ShowDialog();
            });
        }

        /// <summary>
        /// Realiza una pregunta al usuario.
        /// </summary>
        /// <param name="title">Título del mensaje.</param>
        /// <param name="message">Contenido de la pregunta.</param>
        /// <returns>
        /// <see langword="true"/> si el usuario ha dicho que sí al cuadro
        /// de diálogo, <see langword="false"/> en caso contrario.
        /// </returns>
        public static bool Ask(string title, string message)
        {
            return Application.Current.Dispatcher.Invoke(() => {
                var m = new MessageSplash();
                m.DataContext = new MessageSplashViewModel(m, title, message, MessageType.Question);
                m.ShowDialog();
                return ((MessageSplashViewModel)m.DataContext).Result;
            });
        }

        /// <summary>
        /// Realiza una pregunta al usuario.
        /// </summary>
        /// <param name="title">Título del mensaje.</param>
        /// <returns>
        /// <see langword="true"/> si el usuario ha dicho que sí al cuadro
        /// de diálogo, <see langword="false"/> en caso contrario.
        /// </returns>
        public static bool Ask(string title)
        {
            return Ask(title, "¿Está seguro que desea realizar esta operación?");
        }
    }
}