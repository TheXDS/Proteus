/*
Copyright © 2017-2020 César Andrés Morgan
Licenciado para uso interno solamente.
*/

using BarcodeLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using TheXDS.MCART.Types.Extensions;
using TheXDS.Proteus.Dialogs;
using TheXDS.Proteus.Models;
using TheXDS.Proteus.Models.Base;
using TheXDS.Proteus.Plugins;
using TheXDS.Proteus.ViewModels.Base;
using TheXDS.Proteus.Widgets;
using static TheXDS.Proteus.FacturacionUi.Lib.BarcodeHelper;

namespace TheXDS.Proteus.FacturacionUi.Tools
{
    public class FacturableCrudTool : CrudTool<Facturable>
    {
        public FacturableCrudTool() : base(CrudToolVisibility.Selected)
        {
        }

        public override IEnumerable<Launcher> GetLaunchers(IEnumerable<Type> models, ICrudViewModel? vm)
        {
            yield return Launcher.FromMethod(
                "Generar código de barras",
                $"Permite generar un código de barras para este ítem.",
                GenBarcode, () => vm);

            yield return Launcher.FromMethod(
                "Clonar y editar",
                "Crea una copia de este ítem, y permite editarlo. Útil para crear nuevos SKU de ítems similares.",
                GenNewItem, ()=> vm);
        }

        private void GenNewItem(ICrudViewModel vm)
        {
            var entity = (Facturable)vm!.Selection!;
            var copy = entity.GetType().New<Facturable>();
            CopyInfo(entity, copy);
            vm.CreateNewFrom(copy);
        }

        private static void CopyInfo(Facturable entity, Facturable copy)
        {
            copy.Name = entity.Name;
            copy.Isv = entity.Isv;
            copy.Precio = entity.Precio;
            copy.Category = entity.Category;
            if (entity is Producto p1 && copy is Producto p2)
            {
                p2.Description = p1.Description;
                p2.Picture = p1.Picture;
                p2.StockMin = p1.StockMin;
                p2.StockMax = p1.StockMax;
                p2.ExpiryDays = p1.ExpiryDays;
            }
        }

        private void GenBarcode(ICrudViewModel? vm)
        {
            var m = TYPE.CODE128B;
            if (!InputSplash.Get("Seleccione el tipo de código de barra a generar", ref m)) return;
            try
            {
                var f = (Facturable)vm!.Selection!;
                var t = f is Producto p ? p.Batches.OfType<SerialBatch>().Sum(p => p.Qty) : 1;
                var pd = new PrintDialog();
                if (pd.ShowDialog() ?? false)
                {
                    pd.PrintVisual(GenerateBarcodes(m, f), $"Código de barras {m.NameOf()} para {vm!.SelectedElement!.Description.FriendlyName} {vm!.Selection!.StringId} - {App.Info.Name}");
                }
            }
            catch (Exception ex)
            {
                Proteus.MessageTarget?.Warning($"No se pudo utilizar el tipo {m.NameOf()} para generar el código de barra: {ex.Message}");
            }
        }
    }
}
