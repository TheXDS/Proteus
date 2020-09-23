using System.Linq;
using System.Windows;
using TheXDS.MCART.Types.Base;
using TheXDS.Proteus.Api;
using TheXDS.Proteus.Models;
using TheXDS.Proteus.Plugins;

namespace TheXDS.Proteus.FacturacionUi.ViewModels
{
    public class NewPaymentViewModel : NotifyPropertyChanged
    {
        private decimal? _automatic;
        private string? _tag;
        private decimal _amount;
        private readonly FacturadorViewModel _parent;
        private PaymentSource? _source;

        /// <summary>
        ///     Obtiene o establece el valor Source.
        /// </summary>
        /// <value>El valor de Source.</value>
        public PaymentSource? Source
        {
            get => _source;
            set
            {
                if (!Change(ref _source, value)) return;

                var a = value?.Automatic(_parent.CurrentFactura);
                if (a?.IsInvalid ?? false)
                {
                    _parent.SelectedPayment = this;
                    _parent.RemovePaymentCommand.Execute(this);
                    return;
                }
                if (a?.Amount > 0m)
                {
                    Automatic = a.Amount;
                    _parent.RefreshPayments();
                }
                else if (Amount == 0m)
                {
                    Amount = value is { } ? _parent.Vuelto : 0m;
                }
                Tag = a?.Tag;
            }
        }

        /// <summary>
        ///     Obtiene o establece el valor Tag.
        /// </summary>
        /// <value>El valor de Tag.</value>
        public string? Tag
        {
            get => _tag;
            set => Change(ref _tag, value);
        }

        /// <summary>
        ///     Obtiene o establece el valor Amount.
        /// </summary>
        /// <value>El valor de Amount.</value>
        public decimal Amount
        {
            get => Editable ? _amount : Automatic!.Value;
            set
            {
                if (Change(ref _amount, value)) _parent.RefreshPayments();
            }
        }

        /// <summary>
        ///     Obtiene o establece el valor Automatic.
        /// </summary>
        /// <value>El valor de Automatic.</value>
        public decimal? Automatic
        {
            get => _automatic;
            set => Change(ref _automatic, value);
        }

        public NewPaymentViewModel(FacturadorViewModel parent)
        {
            RegisterPropertyChangeBroadcast(nameof(Editable), nameof(EditableV), nameof(NotEditableV));
            RegisterPropertyChangeTrigger(nameof(Editable), nameof(Source), nameof(Automatic));
            RegisterPropertyChangeBroadcast(nameof(Automatic), nameof(Amount));

            _parent = parent;
        }
        public NewPaymentViewModel(FacturadorViewModel parent, Payment payment) : this(parent)
        {
            Source = FacturaService.PaymentSources.FirstOrDefault(p => p.Guid == payment.Source);
            Amount = payment.Amount;
        }

        public bool Editable => !Automatic.HasValue || Automatic > 0m;

        public Visibility EditableV => Editable ? Visibility.Visible : Visibility.Collapsed;
        public Visibility NotEditableV => Editable ? Visibility.Collapsed : Visibility.Visible;

        public static implicit operator NewPaymentViewModel(FacturadorViewModel parent) => new NewPaymentViewModel(parent);
    }
}
