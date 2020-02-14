﻿using TheXDS.Proteus.Crud.Base;
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
            Property(p => p.Kind).Important("Tipo de cuenta");
            ObjectProperty(p => p.Cuenta).Selectable().Required().Important("Auxiliar contable de registro");
        }
    }

}