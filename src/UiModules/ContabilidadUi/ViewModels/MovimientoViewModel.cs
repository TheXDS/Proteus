using System.Globalization;
using System.Linq;
using TheXDS.MCART.ViewModel;
using TheXDS.Proteus.Crud.Base;
using TheXDS.Proteus.Models;

namespace TheXDS.Proteus.ContabilidadUi.ViewModels
{

    /// <summary>
    /// Clase base personalizada para el ViewModel recompilado que se utilizará
    /// dentro del Crud generado para el modelo
    /// <see cref="Partida"/>.
    /// </summary>
    public class PartidaViewModel : ViewModel<Partida>
    {
        /// <summary>
        /// Obtiene el valor de cuadre de la partida.
        /// </summary>
        public decimal Cuadre => Entity is { } e && e.Movimientos.Any() ? e.Movimientos.Sum(p => p.RawValue) : 0m;


        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// <see cref="PartidaViewModel"/>.
        /// </summary>
        public PartidaViewModel()
        {
            RegisterPropertyChangeBroadcast(nameof(Partida.Movimientos), nameof(Cuadre));
        }
    }


    /// <summary>
    /// Clase base personalizada para el ViewModel recompilado que se utilizará
    /// dentro del Crud generado para el modelo
    /// <see cref="Empresa"/>.
    /// </summary>
    public class EmpresaViewModel : ViewModel<Empresa>
    {
        /// <summary>
        /// Permite seleccionar un molde opcional para generar el árbol
        /// contable.
        /// </summary>
        public Molde? FromMolde { get; set; }

        /// <summary>
        /// Obtiene un valor que indica si es posible generar el árbol contable
        /// para una nueva entidad.
        /// </summary>
        public bool CanAddMolde => Entity?.IsNew ?? false;
    }


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