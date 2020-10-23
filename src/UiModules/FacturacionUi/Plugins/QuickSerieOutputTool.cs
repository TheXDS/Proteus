/*
Copyright © 2017-2020 César Andrés Morgan
Licenciado para uso interno solamente.
*/

using System;
using System.Collections.Generic;
using TheXDS.MCART.Types.Extensions;
using TheXDS.Proteus.Api;
using TheXDS.Proteus.Models;
using TheXDS.Proteus.ViewModels.Base;
using TheXDS.Proteus.Widgets;

namespace TheXDS.Proteus.Plugins
{
    public class QuickSerieOutputTool : CrudTool<Batch>
    {
        public QuickSerieOutputTool() : base(CrudToolVisibility.NotEditing)
        {
        }
        public override IEnumerable<Launcher> GetLaunchers(IEnumerable<Type> models, ICrudViewModel? vm)
        {
            yield return Launcher.FromMethod(
                "Salida rápida de números de serie",
                "Permite sacar ítems del inventario con un conjunto de números de serie escaneados rápidamente.",
                SellSerial, () => vm);
        }

        private async void SellSerial(ICrudViewModel? vm)
        {
            var sn = new HashSet<SerialNum>(new IdComparer<SerialNum>());
            while (true)
            {
                string? s = null;
                if (!Proteus.InputTarget?.Get($"Escanee el número de serie a sacar (vacío para continuar, ítems escaneados: {sn.Count})", ref s) ?? false) return;
                if (s.IsEmpty()) break;
                if (Proteus.Service<FacturaService>()!.Get<SerialNum>(s) is { } b)
                {
                    sn.Add(b);
                }
            }
            if (sn.Count == 0) return;

            Cliente? c = null;
            if (!Proteus.InputTarget!.Get("Seleccione el cliente al cual se han vendido los ítems", ref c) || c is null) return;
            foreach (var j in sn)
            {
                j.Sold = DateTime.Now;
                j.Warranty = new Warranty
                {
                    Cliente = c,
                    Timestamp = j.Sold.Value,
                    Void = j.Parent.CalcFrom()
                };
            }
            await Proteus.Service<FacturaService>()!.SaveAsync();            
        }
    }
}
