using TheXDS.Proteus.Crud.Base;
using TheXDS.Proteus.Models;

namespace TheXDS.Proteus.FacturacionUi.Crud
{
    /// <summary>
    /// Describe las propiedades Crud para el modelo
    /// <see cref="CajaDrop"/>.
    /// </summary>
    public class CajaDropCrudDescriptor : CrudDescriptor<CajaDrop>
    {
        /// <summary>
        /// Describe las propiedades Crud para el modelo
        /// <see cref="CajaDrop"/>.
        /// </summary>
        protected override void DescribeModel()
        {
            OnModuleMenu(Annotations.InteractionType.AdminTool);
            FriendlyName("Salida de efectivo de caja");
            Property(p => p.Timestamp).AsListColumn().Hidden();
            ObjectProperty(p => p.Parent).Selectable().Required().Label("Sesión de caja").AsListColumn();
            NumericProperty(p => p.Amount).Positive().Required().Label("Monto").AsListColumn();
            TextProperty(p => p.Concept).Big().Required().Label("Concepto");
            ShowAllInDetails();
        }
    }
}