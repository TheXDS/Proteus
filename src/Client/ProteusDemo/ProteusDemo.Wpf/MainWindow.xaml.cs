using TheXDS.Ganymede.Controls;
using TheXDS.Ganymede.Helpers;
using TheXDS.Ganymede.Services;
using TheXDS.MCART.Types.Base;
using TheXDS.Triton.InMemory.Services;
using TheXDS.Triton.Services;
using Sp = TheXDS.Proteus.Shared.Globals;

namespace TheXDS.Proteus;

/// <summary>
/// Interaction logic for <see cref="MainWindow"/>.
/// </summary>
public partial class MainWindow : ModernWindow, ICloseable
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindow"/> class.
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();

        var tf = new InMemoryTransFactory();
        var tc = new TransactionConfiguration();
        var ui = new DispatcherUiThreadProxy();
        UiThread.SetProxy(ui);
        Sp.Pool.RegisterNow(ui);
        Sp.Pool.RegisterNow(new TritonService(tc, tf));
    }
}
