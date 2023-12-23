#pragma warning disable CS1591

using TheXDS.Ganymede.ViewModels;

namespace TheXDS.Proteus.ViewModels.WizardTest;

public class Wizard1ViewModel : WizardViewModel<WizardDemoState>
{
    public Wizard1ViewModel()
    {
        AddNextInteraction();
        AddCancelInteraction();
    }
}
