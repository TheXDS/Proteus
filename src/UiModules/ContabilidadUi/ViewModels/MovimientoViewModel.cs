using System;
using System.Collections.Generic;
using System.Text;
using TheXDS.MCART.ViewModel;
using TheXDS.Proteus.Models;

namespace TheXDS.Proteus.ContabilidadUi.ViewModels
{
    public abstract class MovimientoViewModel : DynamicViewModel<Movimiento>
    {
        private decimal _debe;
        private decimal _haber;

        /// <summary>
        ///     Obtiene o establece el valor Debe.
        /// </summary>
        /// <value>El valor de Debe.</value>
        public decimal Debe
        {
            get => _debe;
            set
            {
                if (Change(ref _debe, value) && Entity is { } e)
                {
                    _haber = 0m;
                    e.RawValue = value;
                    Notify(nameof(e.RawValue));                    
                }
            }
        }


        /// <summary>
        ///     Obtiene o establece el valor Haber.
        /// </summary>
        /// <value>El valor de Haber.</value>
        public decimal Haber
        {
            get => _haber;
            set
            {
                if (Change(ref _haber, value) && Entity is { } e)
                {
                    _debe = 0m;
                    e.RawValue = -value;
                    Notify(nameof(e.RawValue));
                }
            }
        }

        public MovimientoViewModel()
        {
            RegisterPropertyChangeBroadcast(nameof(Movimiento.RawValue), nameof(Debe), nameof(Haber));
        }
    }
}
