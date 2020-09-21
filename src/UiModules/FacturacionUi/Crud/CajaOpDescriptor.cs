using System;
using System.Linq;
using TheXDS.MCART.Types.Base;
using TheXDS.Proteus.Api;
using TheXDS.Proteus.Crud.Base;
using TheXDS.Proteus.Dialogs;
using TheXDS.Proteus.FacturacionUi.Modules;
using TheXDS.Proteus.Models;
using TheXDS.Proteus.ViewModels.Base;

namespace TheXDS.Proteus.FacturacionUi.Crud
{
    /// <summary>
    /// Describe las propiedades Crud para el modelo
    /// <see cref="CajaOp"/>.
    /// </summary>
    public class CajaOpDescriptor : CrudDescriptor<CajaOp>
    {
        /// <summary>
        /// Describe las propiedades Crud para el modelo
        /// <see cref="CajaOp"/>.
        /// </summary>
        protected override void DescribeModel()
        {
            OnModuleMenu(Annotations.InteractionType.AdminTool);
            FriendlyName("Sesión de caja");

            CanEdit(p => p.CloseTimestamp is null);
            CanDelete(p => p.Facturas.Count == 0);
            CanCreate(_ => !FacturaService.IsCajaOpOpen);

            ObjectProperty(p => p.Estacion).Selectable().AsListColumn().ShowInDetails().Label("Estación");
            ObjectProperty(p => p.Cajero).Selectable().AsListColumn().ShowInDetails();
            NumericProperty(p => p.OpenBalance).Positive().Label("Balance de apertura").AsListColumn().ShowInDetails();

            ListProperty(p => p.Facturas).Creatable();
            ListProperty(p => p.Drops).Creatable().Label("Salidas de efectivo");

            Property(p => p.TotalFacturas).Label("Total facturado").AsListColumn().ShowInDetails().ReadOnly();
            Property(p => p.TotalEfectivo).Label("Total facturado (sólo efectivo)").AsListColumn().ShowInDetails().ReadOnly();

            Property(p => p.CloseTimestamp).Label("Fecha/hora de cierre").AsListColumn().ShowInDetails().ReadOnly();
            Property(p => p.CloseBalance).Label("Balance de cierre").AsListColumn().ShowInDetails().ReadOnly();
            CustomAction("Cerrar sesión de caja", OnCloseSession);
        }

        private void OnCloseSession(CajaOp cajaOp, NotifyPropertyChangeBase vm)
        {
            FacturaService.MakeCierreCaja(cajaOp, () => (vm as ICrudViewModel)?.SaveCommand.Execute(cajaOp));
        }
    }
}