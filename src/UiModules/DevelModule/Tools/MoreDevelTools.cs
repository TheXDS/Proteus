/*
Copyright © 2017-2020 César Andrés Morgan
Licenciado para uso interno solamente.
*/

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TheXDS.MCART.Attributes;
using TheXDS.MCART.PluginSupport.Legacy;
using TheXDS.MCART.Types.Extensions;
using TheXDS.Proteus.Crud;
using TheXDS.Proteus.DevelModule.Resources;
using TheXDS.Proteus.Plugins;
using TheXDS.Proteus.ViewModels.Base;

namespace TheXDS.Proteus.DevelModule.Tools
{
    [Name("Herramientas adicionales de desarrollo")]
    public class MoreDevelTools : Tool
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
            if (msgs.Any())
            {
                Proteus.AlertTarget?.Alert("Limpieza de caché de Cruds Generados", 
                    $"Se ha realizado una operación de limpieza de Cruds generados " +
                    $"en la aplicación. Se ha liberado los siguientes elementos: {Environment.NewLine}  · " +
                    $"{string.Join($"{Environment.NewLine}  · ", msgs)}");
            }
        }

        [InteractionItem, Name("📂⚙"), Description("Carga manualmente un ensamblado.")]
        public void MauallyLoadAssembly(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                Filter = "Ensamblados|*.dll"
            };
            if (ofd.ShowDialog() ?? false) 
            {
                try
                {
                    Assembly.LoadFile(ofd.FileName);
                    Proteus.ReloadSettings(Proteus.Settings!);
                }
                catch (Exception ex)
                {
                    Proteus.MessageTarget?.Warning(ex);
                }
            }
        }

        [InteractionItem, Name("⛑"), Description("Cambia el contexto de ejecución de la aplicación a modo seguro en caliente.")]
        public async void GotoSafeMode(object sender, EventArgs e)
        {
            if (!Proteus.InteractiveMt?.Ask(Strings.HotSfmQuestion, Strings.HotSfmWarning) ?? false) return;
            var s = Proteus.Settings;
            var r = Proteus.CommonReporter;
            r?.UpdateStatus(Strings.HotSfmChanging);
            Proteus.DisposeSettings();
            await Proteus.SafeInit(s);
            (r ?? Proteus.CommonReporter)?.Done();
        }
    }
}
