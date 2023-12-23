using System.Windows;
using TheXDS.Ganymede.Helpers;
using TheXDS.Ganymede.Services;
using TheXDS.Triton.InMemory.Services;
using TheXDS.Triton.Services;
using Sp = TheXDS.ServicePool.ServicePool;

namespace TheXDS.Proteus;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    static App()
    {
        var tf = new InMemoryTransFactory();
        var tc = new TransactionConfiguration();
        var ui = new DispatcherUiThreadProxy();
        UiThread.SetProxy(ui);
        Sp.CommonPool.RegisterNow(ui);
        Sp.CommonPool.RegisterNow(new TritonService(tc, tf));
    }
}
