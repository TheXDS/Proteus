using TheXDS.Proteus.ContabilidadUi.Modules;
using TheXDS.Proteus.Crud.Base;
using TheXDS.Proteus.Models;

namespace TheXDS.Proteus.ContabilidadUi.Crud
{
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
            OnModuleMenu(Annotations.InteractionType.AdminTool);
            FriendlyName("Cuenta por pagar");
            ObjectProperty(p => p.Empresa).Selectable().Required().Important().Default(ContabilidadModule.ModuleStatus?.ActiveEmpresa);
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