/*
Copyright © 2017-2019 César Andrés Morgan
Licenciado para uso interno solamente.
*/

using System;
using System.Threading.Tasks;
using TheXDS.MCART.Attributes;
using TheXDS.MCART.PluginSupport.Legacy;
using TheXDS.MCART.Types;
using TheXDS.MCART.Types.Extensions;
using TheXDS.Proteus.Annotations;
using TheXDS.Proteus.Api;
using TheXDS.Proteus.Component;
using TheXDS.Proteus.Dialogs;
using TheXDS.Proteus.Models;
using TheXDS.Proteus.Pages;
using TheXDS.Proteus.Plugins;
using TheXDS.Proteus.Reporting;
using TheXDS.Proteus.UiDemo.Pages;
using static TheXDS.Proteus.Annotations.InteractionType;

[assembly: Name("UI Demo")]

namespace TheXDS.Proteus
{
    public class UiDemoModule : UiModule
    {
        public UiDemoModule()
        {
            App.UiInvoke(SetupDashboard);
        }

        private void SetupDashboard()
        {
            ModuleDashboard = Misc.AppInternal.BuildWarning(
                "Este módulo está destinado únicamente a pruebas y al equipo de" +
                " desarrollo de Proteus, por lo que no debe ser distribuido en un" +
                " entorno de producción. César Morgan se absuelve de toda" +
                " responsabilidad por los daños que el uso indebido que este" +
                " módulo pueda causar.");
        }

        [InteractionItem, Essential, InteractionType(Operation)]
        public void TestUi(object sender, EventArgs e)
        {
            Proteus.MessageTarget?.Show("Test");
        }

        [InteractionItem, Essential, InteractionType(Operation)]
        public void OpenTestPage(object sender, EventArgs e)
        {
            Host.OpenPage<TestPage>();
        }
        [InteractionItem, InteractionType(AdminTool)]
        public void OpenServiceInfoPage(object sender, EventArgs e)
        {
            Host.OpenPage<DiagnosticsPage>();
        }

        [InteractionItem, Essential, InteractionType(Operation)]
        public void OpenUiTestPage(object sender, EventArgs e)
        {
            Host.OpenPage<UITestPlayground>();
        }

        [InteractionItem, InteractionType(Operation)]
        public void OpenTestMessage(object sender, EventArgs e)
        {
            Proteus.MessageTarget?.Show("Este es un mensaje de prueba");
            Proteus.MessageTarget?.Info("Este es un mensaje de prueba");
            Proteus.MessageTarget?.Warning("Este es un mensaje de prueba");
            Proteus.MessageTarget?.Stop("Este es un mensaje de prueba");
            Proteus.MessageTarget?.Error("Este es un mensaje de prueba");
            Proteus.MessageTarget?.Critical("Este es un mensaje de prueba");
        }

        [InteractionItem, InteractionType(Operation)]
        public void TestQuestion(object sender, EventArgs e)
        {
            
            var r = MessageSplash.Ask("¿Funciona bien Proteus?","Esta es una ventana de prueba. ¿Ha funcionado bien?");
            Proteus.MessageTarget?.Info(r ? "Proteus funcionó bien c:" : "Nooooo!! Proteus no funcionó :c");
        }

        [InteractionItem, InteractionType(Operation)]
        public void TestInputSplash(object sender, EventArgs e)
        {
            static bool Test<T>()
            {
                var r = InputSplash.GetNew<T>("Introduzca un valor de prueba", out var v);
                if (r)
                {
                    Proteus.MessageTarget?.Show("Prueba de InputSplash", v?.ToString());
                }
                return r;
            }

            _ = Test<int>() &&
            Test<string>() &&
            Test<decimal>() &&
            Test<DayOfWeek>() &&
            Test<DaysOfWeek>() &&
            Test<bool>() &&
            Test<DateTime>() &&
            Test<Range<DateTime>>();
        }

        [InteractionItem, InteractionType(Operation)]
        public void TestElevation(object sender, EventArgs e)
        {
            var s = Proteus.Service<UserService>()!;
            if (s.IsElevated && (Proteus.InteractiveMt?.Ask("Existe una elevación activa. ¿Desea revocarla primero?") ?? true))
            {
                s.Revoke();
            }

            if (s.CanRunService(SecurityFlags.Root) ?? false)
            {
                Proteus.MessageTarget?.Info($"Se ejecutó una operación administrativa sin requerir de una elevación. IsElevated: {s.IsElevated}");
            }
            else if (s.Elevate(SecurityFlags.Root))
            {
                Proteus.MessageTarget?.Info($"Se ejecutó una operación administrativa luego de elevar los permisos del usuario. Sesión {s.Session}");
            }
            else
            {
                Proteus.MessageTarget?.Stop("Neles pasteles...");
            }
        }

        [InteractionItem, InteractionType(Reports)]
        public void ReportTest(object sender, EventArgs e)
        {
            var model = typeof(User);
            var fs = new[]
            {
                new ContainsFilter(){ Value = "ev", Property = model.GetProperty("Id",typeof(string)) }
            };

            var q = QueryBuilder.BuildQuery(model, fs);

            var fd = ReportBuilder.MakeReport("Test");
            ReportBuilder.MakeTable(fd, q, new[] { model.GetProperty("Id",typeof(string)), model.GetProperty("Name"), model.GetProperty("DefaultGranted") });
            fd.Print("Test");
        }

        [InteractionItem, InteractionType(Operation)]
        public async void TestReporter(object? sender, EventArgs e)
        {
            Proteus.CommonReporter?.UpdateStatus("Estado indeterminado");
            await Task.Delay(3000);
            Proteus.CommonReporter?.UpdateStatus(20,"Operación importante...");
            await Task.Delay(1000);
            Proteus.CommonReporter?.UpdateStatus(40, "Operación importante...");
            await Task.Delay(1000); 
            Proteus.CommonReporter?.UpdateStatus(60, "Operación importante...");
            await Task.Delay(1000);
            Proteus.CommonReporter?.UpdateStatus(80, "Operación importante...");
            await Task.Delay(1000);
            Proteus.CommonReporter?.UpdateStatus(100, "Operación importante...");
            await Task.Delay(1000);
            Proteus.CommonReporter?.Done();
        }
    }
}