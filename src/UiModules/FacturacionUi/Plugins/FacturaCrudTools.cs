/*
Copyright © 2017-2020 César Andrés Morgan
Licenciado para uso interno solamente.
*/

using System;
using System.Collections.Generic;
using System.Text;
using TheXDS.Proteus.Api;
using TheXDS.Proteus.FacturacionUi.Modules;
using TheXDS.Proteus.Models;
using TheXDS.Proteus.ViewModels.Base;
using TheXDS.Proteus.Widgets;

namespace TheXDS.Proteus.Plugins
{
    public class SerialNumLookupTool : CrudTool<Producto>
    {
        public SerialNumLookupTool():base(CrudToolVisibility.Unselected)
        {
        }
        public override IEnumerable<Launcher> GetLaunchers(IEnumerable<Type> models, ICrudViewModel? vm)
        {
            yield return Launcher.FromMethod(
                "Lookup S/N",
                "Permite buscar información sobre un ítem por medio de su número de serie.",
                LookupSerial);

        }

        private void LookupSerial()
        {
            string? s = null;
            if (!Proteus.InputTarget?.Get("", ref s) ?? false) return;
            var e = Proteus.Service<FacturaService>()!.Get<SerialNum>(s!);
            if (e is { })
            {
                var sb = new StringBuilder();
                sb.AppendLine($"SKU: {e.Parent.Item.Id}");
                sb.AppendLine($"Producto: {e.Parent.Item.Name}");
                sb.AppendLine($"Detalles: {e.Parent.Item.Description}");
                sb.AppendLine($"Notas de este ítem: {e.Notes}");

                Proteus.MessageTarget?.Show(e.Parent.Item.Name,sb.ToString());
            }
        }
    }

    public class FacturaCrudTools : CrudTool<Factura>
    {
        public FacturaCrudTools() : base(CrudToolVisibility.EditAndSelected)
        {
        }

        public override IEnumerable<Launcher> GetLaunchers(IEnumerable<Type> models, ICrudViewModel vm)
        {
            yield return Launcher.FromMethod(
                "Imprimir factura",
                "Permite imprimir una copia de la factura.",
                OnPrint, () => vm.Selection as Factura);

            yield return Launcher.FromMethod("Anular factura", "Permite anular una factura.", OnNullify, vm);
        }

        private void OnPrint(Factura? obj)
        {
            if (obj is null) return;
            if (obj.Impresa)
            {
                FacturaService.PrintFactura(obj, App.Module<FacturacionModule>()?.Interactor);
            }
            else
            {
                FacturaService.AddFactura(obj, true, App.Module<FacturacionModule>()?.Interactor);
            }
        }

        private void OnNullify(ICrudViewModel? obj)
        {
            if (obj is null) return;
            if (!(obj.Selection is Factura f)) return;
            f.Nula = true;
            obj.SaveCommand.Execute(f);
        }
    }
}
