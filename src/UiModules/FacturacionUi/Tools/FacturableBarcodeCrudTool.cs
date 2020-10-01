/*
Copyright © 2017-2020 César Andrés Morgan
Licenciado para uso interno solamente.
*/

using BarcodeLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Documents;
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
    public class FacturableBarcodeCrudTool : CrudTool<Facturable>
    {
        public FacturableBarcodeCrudTool() : base(CrudToolVisibility.Unselected)
        {

        }

        public override IEnumerable<Launcher> GetLaunchers(IEnumerable<Type> models, ICrudViewModel? vm)
        {
            yield return Launcher.FromMethod(
                "Generar códigos de barras",
                $"Permite generar un código de barras para todos los ítems.",
                () => GenBarcodes(vm, p => p.EnumerableResults.OfType<Facturable>()));
            yield return Launcher.FromMethod(
                "Generar códigos de barras",
                $"Permite generar un código de barras para todos los ítems en existencia.",
                () => GenBarcodes(vm, o => o.EnumerableResults.OfType<Producto>().SelectMany(p => Multip(p, p.Batches.OfType<SerialBatch>().Sum(p => p.Qty)))));
        }

        private IEnumerable<Producto> Multip(Producto p, int v)
        {
            for (var j = 0; j < v; j++) yield return p;
        }

        private void GenBarcodes(ICrudViewModel? vm, Func<ISearchViewModel,IEnumerable<Facturable>> query)
        {
            var m = TYPE.CODE128B;
            if (!InputSplash.Get("Seleccione el tipo de código de barra a generar", ref m)) return;
            try
            {
                /*
                var pd = new PrintDialog();
                if (pd.ShowDialog() ?? false)
                {
                    //pd.PrintVisual(GenerateBarcodes(m, vm is ISearchViewModel bb ? query(bb) : Array.Empty<Facturable>().AsEnumerable()), );
                }
                */
                var fd = new FlowDocument();
                fd.SetDpi(new System.Windows.DpiScale(1200, 1200));
                foreach (var j in vm is ISearchViewModel bb ? query(bb) : Array.Empty<Facturable>().AsEnumerable())
                {
                    fd.Blocks.Add(new BlockUIContainer(GenerateBarcodeBlock(m, j)));
                }
                fd.Print($"Código de barras {m.NameOf()} - {App.Info.Name}");
            }
            catch (Exception ex)
            {
                Proteus.MessageTarget?.Warning($"No se pudo utilizar el tipo {m.NameOf()} para generar el código de barra: {ex.Message}");
            }
        }

    }
}
