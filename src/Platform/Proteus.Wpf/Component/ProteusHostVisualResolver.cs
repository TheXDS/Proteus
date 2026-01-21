using TheXDS.Ganymede.Component;
using TheXDS.Ganymede.Helpers;
using TheXDS.Ganymede.Types.Base;
using TheXDS.Proteus.ViewModels;
using TheXDS.Proteus.Views;

namespace TheXDS.Proteus.Component;

/// <summary>
/// Resolves a <see cref="ProteusHostViewModel"/> to a
/// <see cref="ProteusHostView"/>.
/// </summary>
public class ProteusHostVisualResolver : IVisualResolver<ProteusHostView>
{
    public ProteusHostView? Resolve(IViewModel viewModel)
    {
        if (viewModel is ProteusHostViewModel)
        {
            return UiThread.Invoke(() => new ProteusHostView
            {
                DataContext = viewModel
            });
        }
        return null;
    }
}
