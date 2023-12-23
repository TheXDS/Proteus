#pragma warning disable CS1591

using TheXDS.Ganymede.ViewModels;

namespace TheXDS.Proteus.ViewModels.WizardTest;

public class Wizard4ViewModel : WizardViewModel<WizardDemoState>
{
    public Wizard4ViewModel()
    {
        AddNextInteraction("Finish");
    }
}
