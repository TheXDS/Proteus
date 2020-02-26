using System.Collections.Generic;
using System.Linq;
using TheXDS.MCART.ViewModel;
using TheXDS.Proteus.Api;
using TheXDS.Proteus.Component;
using TheXDS.Proteus.ContabilidadUi.Modules;
using TheXDS.Proteus.Models;
using TheXDS.Proteus.ViewModels.Base;

namespace TheXDS.Proteus.ContabilidadUi.ViewModels
{
    public class PayCtaWithViewModel : PageViewModel, IReadEntityViewModel<CtaXPagar>
    {
        private bool _withBank;
        private bool _withContab;
        private CuentaBanco _selectedCuenta;
        private decimal _monto;

        public PayCtaWithViewModel(ICloseable host, CtaXPagar cta) : base(host)
        {
            PayCommand = new SimpleCommand(OnPay);
            CancelCommand = new SimpleCommand(OnCancel);
            Entity = cta;
        }

        public CtaXPagar Entity { get; }

        /// <summary>
        /// Calcula el total pagado de esta cuenta por pagar.
        /// </summary>
        public decimal Paid => Entity.Payments.Any() ? Entity.Payments.Sum(p => p.Monto) : 0m;
        public decimal Remaining => Entity.Total - Paid;

        /// <summary>
        ///     Obtiene o establece el valor WithBank.
        /// </summary>
        /// <value>El valor de WithBank.</value>
        public bool WithBank
        {
            get => _withBank;
            set => Change(ref _withBank, value);
        }

        /// <summary>
        ///     Obtiene o establece el valor WithContab.
        /// </summary>
        /// <value>El valor de WithContab.</value>
        public bool WithContab
        {
            get => _withContab;
            set => Change(ref _withContab, value);
        }

        public IEnumerable<CuentaBanco> CurrentCuentas => ContabilidadService.CuentasFor(ContabilidadModule.ModuleStatus.ActiveEmpresa!);

        /// <summary>
        ///     Obtiene o establece el valor SelectedCuenta.
        /// </summary>
        /// <value>El valor de SelectedCuenta.</value>
        public CuentaBanco SelectedCuenta
        {
            get => _selectedCuenta;
            set => Change(ref _selectedCuenta, value);
        }

        /// <summary>
        ///     Obtiene o establece el valor Monto.
        /// </summary>
        /// <value>El valor de Monto.</value>
        public decimal Monto
        {
            get => _monto;
            set => Change(ref _monto, value);
        }


        private string _NumRef;

        /// <summary>
        ///     Obtiene o establece el valor NumRef.
        /// </summary>
        /// <value>El valor de NumRef.</value>
        public string NumRef
        {
            get => _NumRef;
            set => Change(ref _NumRef, value);
        }


        /// <summary>
        ///     Obtiene el comando relacionado a la acción Pay.
        /// </summary>
        /// <returns>El comando Pay.</returns>
        public SimpleCommand PayCommand { get; }

        /// <summary>
        ///     Obtiene el comando relacionado a la acción Cancel.
        /// </summary>
        /// <returns>El comando Cancel.</returns>
        public SimpleCommand CancelCommand { get; }

        private void OnPay()
        {
            if (WithBank) PayWithBank();
        }

        private void OnCancel()
        {
            Host?.Close();
        }
        private void PayWithBank()
        {
            Proteus.MessageTarget?.Info("Función no implementada.\nEsta ventana presenta una vista previa de la transacción de pagos de cuentas.");
        }
    }
}