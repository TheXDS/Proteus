/*
Copyright © 2017-2020 César Andrés Morgan
Licenciado para uso interno solamente.
*/

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.Json;
using TheXDS.MCART;
using TheXDS.MCART.Attributes;
using TheXDS.MCART.PluginSupport.Legacy;
using TheXDS.MCART.Types.Extensions;
using TheXDS.Proteus.Config;
using TheXDS.Proteus.Plugins;

namespace TheXDS.Proteus.DevelModule.Tools
{
    public class UpdatePushTool : Tool
    {
        private struct AsmInfo
        {
            public AsmInfo(string name, string version)
            {
                this.name = name;
                this.version = version;
            }

            public string name { get; set; }
            public string version { get; set; }
        }

        [InteractionItem, Name("💎"), Description("Envía el manifiesto de componentes actuales al servidor de actualizaciones de Proteus.")]
        public async void PushUpdates(object? sender, EventArgs e)
        {
            try
            {
                Proteus.CommonReporter?.UpdateStatus("Enviando manifiesto de actualizaciones...");
                var list = AppDomain.CurrentDomain.GetAssemblies().Select(GetInfo).NotNull().ToList();
                list.Add(new AsmInfo
                {
                    name = $"{App.Info.Name}.exe",
                    version = App.Info.InformationalVersion ?? App.Info.Version?.ToString() ?? "1.0.0.0"
                });
                var request = (HttpWebRequest)WebRequest.Create($"{Settings.Default.UpdateServer.TrimEnd('/')}/v1/Update/WriteManifest");
                request.Method = "POST";
                request.ContentType = "application/json";
                await JsonSerializer.SerializeAsync(await request.GetRequestStreamAsync(), list.ToArray());
                var resp = (HttpWebResponse)await request.GetResponseAsync();
                if (resp.StatusCode != HttpStatusCode.OK)
                {
                    Proteus.MessageTarget?.Warning(resp.StatusDescription);
                };
            }
            catch (Exception ex)
            {
                Proteus.MessageTarget?.Error(ex.Message);
            }
            finally 
            {
                Proteus.CommonReporter?.Done();
            }
        }

        private AsmInfo? GetInfo(Assembly? asm)
        {
            if (asm is null) return null;
            var p = asm.GetName();

            // Comprobar la longitud de InformationalVersion ayuda a filtrar ensamblados de .Net Core.
            var v = asm.GetAttr<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
            return !(p.Name is null || v?.Length > 20)
                ? new AsmInfo($"{p.Name}{Path.GetExtension(p.CodeBase)}", v ?? p.Version?.ToString() ?? "1.0.0.0")
                : (AsmInfo?)null;
        }
    }
}
