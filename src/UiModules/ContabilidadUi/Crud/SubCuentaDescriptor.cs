using TheXDS.Proteus.ContabilidadUi.Modules;
using TheXDS.Proteus.Crud.Base;
using TheXDS.Proteus.Models;

namespace TheXDS.Proteus.ContabilidadUi.Crud
{
    /// <summary>
    /// Describe las propiedades Crud para el modelo
    /// <see cref="SubCuenta"/>.
    /// </summary>
    public class SubCuentaDescriptor : CrudDescriptor<SubCuenta>
    {
        protected override void DescribeModel()
        {
            Property(p => p.FullCode).Label("Código de cuenta").AsListColumn().ShowInDetails().Hidden();
            Property(p => p.Name).AsName();
            Property(p => p.Movimientos).OnlyInDetails("Movimientos de la partida");
            ObjectProperty(p => p.Divisa).Selectable().Nullable().ShowInDetails().Label("Divisa de la cuenta");
            Template();
            BeforeSave<Cuenta>(SetPrefix);
        }

        private void SetPrefix(SubCuenta arg1, Cuenta? arg2)
        {
            if (!arg1.IsNew) return;
            arg1.Prefix = arg2?.FreeSubCuentaPrefix ?? 1;
            arg1.Parent ??= arg2;
        }
    }

    /// <summary>
    /// Describe las propiedades Crud para el modelo
    /// <see cref="Banco"/>.
    /// </summary>
    public class BancoCrudDescriptor : CrudDescriptor<Banco>
    {
        /// <summary>
        /// Describe las propiedades Crud para el modelo
        /// <see cref="Banco"/>.
        /// </summary>
        protected override void DescribeModel()
        {
            Property(p => p.Name).AsName();
            this.DescribeContact();
            ListProperty(p => p.Cuentas).Creatable().Required().ShowInDetails();
        }
    }

    /// <summary>
    /// Describe las propiedades Crud para el modelo
    /// <see cref="CuentaBanco"/>.
    /// </summary>
    public class CuentaBancoCrudDescriptor : CrudDescriptor<CuentaBanco>
    {
        /// <summary>
        /// Describe las propiedades Crud para el modelo
        /// <see cref="CuentaBanco"/>.
        /// </summary>
        protected override void DescribeModel()
        {
            VmObjectProperty(p => p.SelectedEmpresa).Selectable();
            ObjectProperty(p => p.DebitoCuenta)
                .Selectable()
                .VmSource<ProveedorXEmpresaViewModel>(p => p.CurrentSubCuentas)
                .Required()
                .Important("Auxiliar de gastos pagados por anticipado");
            ObjectProperty(p => p.CreditoCuenta)
                .Selectable()
                .VmSource<ProveedorXEmpresaViewModel>(p => p.CurrentSubCuentas)
                .Required()
                .Important("Auxiliar de cuentas por pagar");
        }
    }

    /// <summary>
    /// Describe las propiedades Crud para el modelo
    /// <see cref="CtaXPagar"/>.
    /// </summary>
    public class CtaXPagarDescriptor : CrudDescriptor<CtaXPagar>
    {
        /// <summary>
        /// Describe las propiedades Crud para el modelo
        /// <see cref="CtaXPagar"/>.
        /// </summary>
        protected override void DescribeModel()
        {
            FriendlyName("Cuenta por pagar");
            ObjectProperty(p => p.Proveedor).Selectable().Important();
            Property(p => p.Timestamp).Important("Fecha de emisión");
            Property(p => p.RefNum).Required().Important("# de ref. de factura/cuenta");
            NumericProperty(p => p.Total).Positive().Important("Total a pagar");
            Property(p => p.Paid).Important("Pagada").Hidden();
            ObjectProperty(p => p.CreationPartida).Creatable().Required().Important("Partida de costo");
            ObjectProperty(p => p.PaymentPartida).Creatable().Nullable().Important("Partida de pago");
        }
    }

}