/*
Copyright © 2017-2020 César Andrés Morgan
Licenciado para uso interno solamente.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TheXDS.MCART.Attributes;
using TheXDS.MCART.PluginSupport.Legacy;
using TheXDS.MCART.Types.Extensions;
using TheXDS.Proteus.Crud;
using TheXDS.Proteus.Plugins;
using TheXDS.Proteus.ViewModels.Base;

namespace TheXDS.Proteus.DevelModule.Tools
{
    [Name("Herramienta de limpieza rápida de Cruds generados")]

    public class CrudResetTool : Tool
    {
        [InteractionItem, Name("Crud ➡ 🗑"), Description("Limpia la caché en memoria de los Cruds generados.")]
        public void ClearCrudCache(object sender, EventArgs e)
        {
            var msgs = new HashSet<string>();
            foreach (var vm in ProteusViewModel.ActuallyActiveVms.OfType<CrudViewModelBase>())
            {
                if (vm.EditMode)
                { 
                    vm.CancelCommand.Execute(null);
                }
                vm.Selection = null;
                var c = (ICollection<CrudElement>)typeof(CrudViewModelBase).GetProperty("Elements", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(vm)!;
                if (c.Any())
                {
                    msgs.AddRange(c.Select(p => p.Description.FriendlyName));
                    c.Clear();
                }
            }
            Proteus.MessageTarget?.Info($"Operación de limpieza de Crud realizada correctamente. Se liberaron {msgs.Count} elementos construidos.");
            Proteus.AlertTarget?.Alert("Limpieza de caché de Cruds Generados", 
                $"Se ha realizado una operación de limpieza de Cruds generados " +
                $"en la aplicación. Se ha liberado los siguientes elementos: {Environment.NewLine}  · " +
                $"{string.Join($"{Environment.NewLine}  · ", msgs)}");
        }
    }
}
