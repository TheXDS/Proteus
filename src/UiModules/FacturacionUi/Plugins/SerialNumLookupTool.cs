/*
Copyright © 2017-2020 César Andrés Morgan
Licenciado para uso interno solamente.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheXDS.MCART.Types.Extensions;
using TheXDS.Proteus.Api;
using TheXDS.Proteus.FacturacionUi.Modules;
using TheXDS.Proteus.Models;
using TheXDS.Proteus.Pages;
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
            if (!Proteus.InputTarget?.Get("Ingrese o escanee el número de serie a buscar", ref s) ?? false) return;
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

    public class QuickSerieInputTool : CrudTool<Producto>
    {
        public QuickSerieInputTool() : base(CrudToolVisibility.NotEditing)
        {
        }
        public override IEnumerable<Launcher> GetLaunchers(IEnumerable<Type> models, ICrudViewModel? vm)
        {
            yield return Launcher.FromMethod(
                "Ingreso rápido de números de serie",
                "Permite crear un nuevo Batch con un conjunto de números de serie escaneados rápidamente.",
                LookupSerial, () => vm);
        }

        private void LookupSerial(ICrudViewModel? vm)
        {
            var sn = new HashSet<string>();
            while (true)
            {
                string? s = null;
                if (!Proteus.InputTarget?.Get($"Escanee el número de serie a ingresar (vacío para continuar, ítems escaneados: {sn.Count})", ref s) ?? false) return;
                if (s.IsEmpty()) break;
                sn.Add(s!);
            }
            if (sn.Count == 0) return;
            var sb = new SerialBatch { Item = (vm?.Selection as Producto)! };
            sb.Serials.AddRange(sn.Select(q => new SerialNum() { Id = q }));
            App.Module<FacturacionModule>()!.Host.OpenPage(QuickCrudPage.Edit(sb));
        }
    }
}
