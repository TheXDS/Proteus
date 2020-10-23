/*
Copyright © 2017-2020 César Andrés Morgan
Licenciado para uso interno solamente.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using TheXDS.MCART.Types.Extensions;
using TheXDS.Proteus.FacturacionUi.Modules;
using TheXDS.Proteus.Models;
using TheXDS.Proteus.Pages;
using TheXDS.Proteus.ViewModels.Base;
using TheXDS.Proteus.Widgets;

namespace TheXDS.Proteus.Plugins
{
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
