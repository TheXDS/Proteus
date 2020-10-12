/*
Copyright © 2017-2020 César Andrés Morgan
Licenciado para uso interno solamente.
*/

using TheXDS.Proteus.Component;
using SourceChord.FluentWPF;
using System.Windows;
using TheXDS.MCART.ViewModel;
using TheXDS.Proteus.Crud;
using TheXDS.Proteus.Crud.Base;
using System.ComponentModel;
using TheXDS.MCART.Types.Base;
using TheXDS.MCART;

namespace TheXDS.Proteus.Dialogs
{
    /// <summary>
    /// Lógica de interacción para InputSplash.xaml
    /// </summary>
    public partial class InputSplash : AcrylicWindow, ICloseable
    {
        private InputSplash()
        {
            InitializeComponent();
            Loaded += InputSplash_Loaded;
        }

        private void InputSplash_Loaded(object sender, RoutedEventArgs e)
        {
            (DataContext as InputSplashViewModel)?.InputControl?.Focus();
        }

        public static bool GetNew<T>(string prompt, out T value)
        {
            value = default!;
            return Get(prompt, ref value);
        }

        public static bool Get<T>(string prompt, ref T value)
        {
            var descr = new InputSplashDescription
            {
                Label = prompt,
                Property = ReflectionHelpers.GetProperty<InputSplashViewModel<T>>(p => p.InputValue)!,
            };
            var dialog = new InputSplash();
            var vm = new InputSplashViewModel<T>(dialog, descr)
            {
                InputValue = value
            };           
            dialog.DataContext = vm;
            dialog.ShowDialog();
            value = vm.InputValue;
            return !vm.Cancel;
        }
    }

    public class InputSplashTarget : IInputTarget
    {
        public bool Get<T>(string prompt, ref T value)
        {
            return InputSplash.Get<T>(prompt, ref value);
        }

        public bool GetNew<T>(string prompt, out T value)
        {
            return InputSplash.GetNew<T>(prompt, out value);
        }
    }

    public abstract class InputSplashViewModel : NotifyPropertyChanged
    {
        protected readonly ICloseable _host;

        protected InputSplashViewModel(ICloseable host)
        {
            _host = host;
        }

        /// <summary>
        /// Obtiene o establece el valor Title.
        /// </summary>
        /// <value>El valor de Title.</value>
        public string Prompt { get; protected set; }

        /// <summary>
        /// Obtiene un valor que indica si la obtención de un valor ha sido cancelada.
        /// </summary>
        public bool Cancel { get; protected set; }

        /// <summary>
        /// Obtiene el control de edición a utilizar para obtener el valor
        /// introducido por el usuario.
        /// </summary>
        public FrameworkElement InputControl { get; protected set; }

        /// <summary>
        /// Obtiene el comando relacionado a la acción Close.
        /// </summary>
        /// <returns>El comando Close.</returns>
        public SimpleCommand CloseCommand { get; protected set; }

        /// <summary>
        /// Obtiene el comando relacionado a la acción Go.
        /// </summary>
        /// <returns>El comando Go.</returns>
        public SimpleCommand GoCommand { get; protected set; }
    }

    public class InputSplashViewModel<T> : InputSplashViewModel
    {
        private T _inputValue;

        public InputSplashViewModel(ICloseable host, IPropertyDescription description) : base(host)
        {
            CloseCommand = new SimpleCommand(OnClose);
            GoCommand = new SimpleCommand(OnGo);
            InputControl = PropertyMapper.GetMapping(null, description)?.Control;
            Prompt = description.Label;
        }

        private void OnGo()
        {
            _host.Close();
        }

        private void OnClose()
        {
            Cancel = true;
            InputValue = default!;
            _host.Close();
        }

        public T InputValue
        {
            get => _inputValue; 
            set => Change(ref _inputValue, value);
        }
    }
}