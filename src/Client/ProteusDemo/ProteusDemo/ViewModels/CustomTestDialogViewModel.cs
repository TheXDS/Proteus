#pragma warning disable CS1591

using TheXDS.Ganymede.ViewModels;
using TheXDS.MCART.Component;
using St = TheXDS.Proteus.Resources.Strings.ViewModels.CustomTestDialogViewModel;

namespace TheXDS.Proteus.ViewModels;

public class CustomTestDialogViewModel : AwaitableDialogViewModel
{
    private int _RingValue;
    private int _TimesRan;

    public int RingValue
    {
        get => _RingValue;
        set => Change(ref _RingValue, value);
    }

    public int TimesRan
    {
        get => _TimesRan;
        set => Change(ref _TimesRan, value);
    }

    public CustomTestDialogViewModel()
    {
        Icon = "⭕";
        IconBgColor = System.Drawing.Color.Magenta;
        Title = St.CustomDialog;
        Message = St.DlgMessage;

        Interactions.Add(new(new SimpleCommand(OnRunRing), St.Run));
        Interactions.Add(new(new SimpleCommand(CloseDialog), St.Close));
    }

    private async Task OnRunRing()
    {
        IsBusy = true;
        for (; RingValue < 100; RingValue++) { await Task.Delay(50); }
        RingValue = 0;
        TimesRan++;
        IsBusy = false;
    }
}
