using TheXDS.Proteus.Crud.Base;
using TheXDS.Proteus.Models;

namespace TheXDS.Proteus.FacturacionUi.Crud
{
    /// <summary>
    /// Describe las propiedades Crud para el modelo
    /// <see cref="Payment"/>.
    /// </summary>
    public class PaymentDescriptor : CrudDescriptor<Payment>
    {
        /// <summary>
        /// Describe las propiedades Crud para el modelo
        /// <see cref="Payment"/>.
        /// </summary>
        protected override void DescribeModel()
        {
            Property(p => p.ResolvedSource).AsListColumn().ShowInDetails().Label("Origen d pago").ReadOnly();
            NumericProperty(p => p.Amount).Range(decimal.Zero, decimal.MaxValue).Important("Monto");
            Property(p => p.Tag).ShowInDetails().Label("Referencia de pago").ReadOnly();
        }
    }
}