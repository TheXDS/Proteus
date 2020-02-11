using TheXDS.Proteus.Crud.Base;
using TheXDS.Proteus.Models;

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
            Property(p => p.FullCode).Label("Código de cuenta").AsListColumn().ShowInDetails().Hidden();
            Property(p => p.Name).AsName().AsListColumn();
            ListProperty(p => p.Children).Creatable().Important("Cuentas hijas");
            ListProperty(p => p.SubCuentas).Creatable().Important("Sub-cuentas");
            ObjectProperty(p => p.DefaultDivisa).Selectable().Nullable().ShowInDetails().Label("Divisa de la cuenta");
            ShowAllInDetails();
            Template();
            BeforeSave<Cuenta>(SetPrefix);

            CanDelete(c => c.Parent is { } && c.Children.Count == 0 && c.SubCuentas.Count == 0);
        }

        private void SetPrefix(Cuenta arg1, Cuenta? arg2)
        {
            if (!arg1.IsNew) return;
            arg1.Prefix = arg2?.FreeCuentaPrefix ?? 1;
            arg1.Parent ??= arg2;
        }
    }
}