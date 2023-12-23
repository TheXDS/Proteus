#pragma warning disable CS1591

using TheXDS.Ganymede.ViewModels;

namespace TheXDS.Proteus.ViewModels.WizardTest;

public class Wizard2ViewModel : WizardViewModel<WizardDemoState>
{
    public Wizard2ViewModel()
    {
        AddBackInteraction();
        AddNextInteraction("Start");
        AddCancelInteraction();
    }
}
