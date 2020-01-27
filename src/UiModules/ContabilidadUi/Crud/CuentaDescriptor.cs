using System;
using TheXDS.Proteus.Crud.Base;
using TheXDS.Proteus.Models;
using TheXDS.Proteus.Models.Base;

namespace TheXDS.Proteus.ContabilidadUi.Crud
{
    /// <summary>
    /// Describe las propiedades Crud para el modelo
    /// <see cref="Cuenta"/>.
    /// </summary>
    public class CuentaDescriptor : CrudDescriptor<Cuenta>
    {
        /// <summary>
        /// Describe el modelo <see cref="Cuenta"/>.
        /// </summary>
        protected override void DescribeModel()
        {
            Property(p => p.FullCode).Label("Código de cuenta").OnlyInDetails();
            Property(p => p.Name).AsName().AsListColumn();
            ListProperty(p => p.Children).Creatable().Important("Cuentas hijas");
            ListProperty(p => p.SubCuentas).Creatable().Important("Sub-cuentas");
            ShowAllInDetails();
            Template();
            BeforeSave(SetPrefix);

            CanDelete(c => c.Parent is { } && c.Children.Count == 0 && c.SubCuentas.Count == 0);
        }

        private void SetPrefix(Cuenta arg1, ModelBase arg2)
        {
            if (!(arg2 is Cuenta c)) return;
            arg1.Prefix = c.FreeCuentaPrefix;
        }
    }
}