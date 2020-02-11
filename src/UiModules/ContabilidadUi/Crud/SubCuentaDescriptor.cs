using System.Collections.Generic;
using System.Linq;
using TheXDS.MCART.Types;
using TheXDS.Proteus.Api;
using TheXDS.Proteus.ContabilidadUi.Modules;
using TheXDS.Proteus.Crud.Base;
using TheXDS.Proteus.Models;
using TheXDS.Proteus.ViewModels;

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
    /// <see cref="Proveedor"/>.
    /// </summary>
    public class ProveedorDescriptor : CrudDescriptor<Proveedor>
    {
        /// <summary>
        /// Describe las propiedades Crud para el modelo
        /// <see cref="Proveedor"/>.
        /// </summary>
        protected override void DescribeModel()
        {
            OnModuleMenu(Annotations.InteractionType.AdminTool);
            Property(p => p.Name).AsName();
            TextProperty(p => p.Rtn).Mask("9999-9999-999999").Required().Important("RTN");
            this.DescribeContact();
            this.DescribeAddress();
            ListProperty(p => p.Empresas)
                .Creatable()
                .Required()
                .Label("Cuentas contables por empresa")
                .ShowInDetails();
        }
    }

    /// <summary>
    /// Describe las propiedades Crud para el modelo
    /// <see cref="ProveedorXEmpresa"/>.
    /// </summary>
    public class ProveedorXEmpresaDescriptor : CrudDescriptor<ProveedorXEmpresa, ProveedorXEmpresaViewModel>
    {
        /// <summary>
        /// Describe las propiedades Crud para el modelo
        /// <see cref="ProveedorXEmpresa"/>.
        /// </summary>
        protected override void DescribeModel()
        {
            //ObjectProperty(p => p.Proveedor).Selectable();
            ObjectProperty(p => p.Empresa).Selectable().Default(ContabilidadModule.ModuleStatus.ActiveEmpresa!);
            ObjectProperty(p => p.DebitoCuenta).Selectable().VmSource<ProveedorXEmpresaViewModel>(p=>p.CurrentSubCuentas).Required().Important("Auxiliar de gastos pagados por anticipado");
            ObjectProperty(p => p.CreditoCuenta).Selectable().Required().Important("Auxiliar de cuentas por pagar");
        }
        private static ObservableListWrap<SubCuenta> CurrentSubCuentas()
        {
            var e = ContabilidadModule.ModuleStatus.ActiveEmpresa!;
            //return Flatten(e.Activo)
            //    .Concat(Flatten(e.Pasivo))
            //    .Concat(Flatten(e.Patrimonio))
            //    .AsQueryable();
            return null;
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