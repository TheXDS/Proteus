#pragma warning disable CS1591

using TheXDS.Ganymede.ViewModels;

namespace TheXDS.Proteus.ViewModels.WizardTest;

public class Wizard3ViewModel : WizardOperationViewModel<WizardDemoState>, IOperationDialogViewModel
{
    protected override Task RunOperation()
    {
        return Task.Delay(3000);
    }
}
