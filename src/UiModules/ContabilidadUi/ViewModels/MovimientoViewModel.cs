using System.Globalization;
using TheXDS.MCART.ViewModel;
using TheXDS.Proteus.Models;

namespace TheXDS.Proteus.ContabilidadUi.ViewModels
{
    /// <summary>
    /// ViewModel que gestiona propiedades avanzadas y calculadas del modelo
    /// <see cref="Movimiento"/>.
    /// </summary>
    public class MovimientoViewModel : ViewModel<Movimiento>
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

        /// <summary>
        /// Obtiene el valor registrado del movimiento en la divisa específica
        /// de la cuenta.
        /// </summary>
        public string? RealValue
        {
            get
            {
                if (Entity is null) return null;
                return Entity.RawValue.ToString("C", Entity.Cuenta?.Divisa?.Culture ?? CultureInfo.CurrentCulture);
            }
        }

        /// <summary>
        /// Obtiene el valor registrado del movimiento en la divisa local del
        /// sistema, realizando la conversión con la taza de cambio almacenada.
        /// </summary>
        public string? LocalValue
        {
            get
            {
                if (Entity is null) return null;
                return (Entity.RawValue * (Entity.ExchangeRate ?? 1m)).ToString("C", CultureInfo.CurrentCulture);
            }
        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// <see cref="MovimientoViewModel"/>.
        /// </summary>
        public MovimientoViewModel()
        {
            RegisterPropertyChangeBroadcast(nameof(Movimiento.RawValue),
                nameof(Debe),
                nameof(Haber),
                nameof(RealValue),
                nameof(LocalValue));

            RegisterPropertyChangeBroadcast(nameof(Movimiento.Cuenta),
                nameof(RealValue),
                nameof(LocalValue),
                nameof(Movimiento.ExchangeRate));

            RegisterPropertyChangeBroadcast(nameof(Movimiento.ExchangeRate),
                nameof(RealValue),
                nameof(LocalValue));
        }
    }
}