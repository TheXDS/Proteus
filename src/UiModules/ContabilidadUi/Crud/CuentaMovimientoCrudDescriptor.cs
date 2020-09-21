using System;
using TheXDS.Proteus.Crud.Base;
using TheXDS.Proteus.Models;

namespace TheXDS.Proteus.ContabilidadUi.Crud
{
    /// <summary>
    /// Describe las propiedades Crud para el modelo
    /// <see cref="CuentaMovimiento"/>.
    /// </summary>
    public class CuentaMovimientoCrudDescriptor : CrudDescriptor<CuentaMovimiento>
    {
        /// <summary>
        /// Describe las propiedades Crud para el modelo
        /// <see cref="CuentaMovimiento"/>.
        /// </summary>
        protected override void DescribeModel()
        {
            FriendlyName("Transacción bancaria");
            DateProperty(p => p.Timestamp).WithTime().Default(DateTime.Now).Important();
            Property(p => p.Name).AsName("Número de referencia");
            Property(p => p.Kind).Important("Tipo de transacción");
            NumericProperty(p => p.Monto).NonZero().Important();

            ObjectProperty(p => p.Source).Selectable().Creatable().RadioSelectable().Label("Cliente / donante");
            ObjectProperty(p => p.Beneficiario).Selectable().Creatable().RadioSelectable().Label("Beneficiario / proveedor");
            ObjectProperty(p => p.LocalTarget).Selectable().RadioSelectable().Label("Cuenta de banco propia");
            Property(p => p.ExternalTarget).RadioSelectable().Label("Otro tipo de transacción");
        }
    }
}