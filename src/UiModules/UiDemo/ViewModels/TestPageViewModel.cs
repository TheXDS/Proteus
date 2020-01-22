/*
Copyright © 2017-2019 César Andrés Morgan
Licenciado para uso interno solamente.
*/

using System;
using System.Windows.Input;
using TheXDS.MCART.ViewModel;
using TheXDS.Proteus.Component;
using TheXDS.Proteus.ViewModels.Base;

namespace TheXDS.Proteus.UiDemo.ViewModels
{
    public class TestPageViewModel : PageViewModel
    {
        private static int _counter = 0;
        private static readonly Random _rnd = new Random();

        private TestModel? _test;

        public TestModel? Entity
        {
            get => _test;
            set
            {
                _test = value;
                OnPropertyChanged();
            }
        }
        public ICommand FlipCommand { get; }

        public TestPageViewModel(ICloseable host) : base(host)
        {
            Title = $"Página de prueba {_counter++}";
            FlipCommand = new SimpleCommand(OnFlip);
        }
        private void OnFlip()
        {
            Entity = new TestModel { Age = _rnd.Next(0, 100), Name = new string((char)_rnd.Next(65, 110), _rnd.Next(5, 10)) };
        }
    }

    public class TestModel
    {
        public int Age { get; set; }
        public string? Name { get; set; }
    }

}
