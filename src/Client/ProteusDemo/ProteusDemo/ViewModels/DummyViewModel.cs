using TheXDS.Ganymede.Types.Base;
using St = TheXDS.Proteus.Resources.Strings.ViewModels.DummyViewModel;
namespace TheXDS.Proteus.ViewModels;

/// <summary>
/// ViewModel that does nothing.
/// </summary>
public class DummyViewModel : ViewModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DummyViewModel"/> class.
    /// </summary>
    public DummyViewModel()
    {
        Title = St.Title;
    }
}
