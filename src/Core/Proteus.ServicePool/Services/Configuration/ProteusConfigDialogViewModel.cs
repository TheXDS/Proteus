using System.Windows.Input;
using TheXDS.Ganymede.Helpers;
using TheXDS.Ganymede.Models;
using TheXDS.Ganymede.Types.Extensions;
using TheXDS.Ganymede.ViewModels;
using TheXDS.MCART.Component;

namespace TheXDS.Proteus.Services.Configuration;

/// <summary>
/// Base class for all commomn configuration dialogs in Proteus.
/// </summary>
public abstract class ProteusConfigDialogViewModel : AwaitableDialogViewModel<DialogResult<object?>>, IValidatableViewModel
{
    private bool isStateValid;

    /// <inheritdoc/>
    public bool IsStateValid
    {
        get => isStateValid;
        protected set => Change(ref isStateValid, value);
    }

    /// <summary>
    /// Gets a reference to the command used to accept this dialog.
    /// </summary>
    public ICommand OkCommand { get; }

    /// <summary>
    /// Gets a reference to the command used to cancel this dialog.
    /// </summary>
    public ICommand CancelCommand { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProteusConfigDialogViewModel"/> class.
    /// </summary>
    protected ProteusConfigDialogViewModel()
    {
        var cb = CommandBuilder.For(this);
        Interactions.Add(OkCommand = BuildOkCommand(cb), "Ok");
        Interactions.Add(CancelCommand = BuildCancelCommand(cb), "Cancel");
    }

    /// <summary>
    /// If overriden in a derivate class, gets a configuration value that can
    /// be serialized and stored. Otherwise, it returns <see langword="null"/>.
    /// </summary>
    /// <returns>
    /// A serializable value that represents the values configured using this
    /// dialog.
    /// </returns>
    protected virtual object? GetSerializableConfig() => null;

    private static ICommand BuildCancelCommand(CommandBuilder<ProteusConfigDialogViewModel> cb)
    {
        return cb.BuildResultCommand(new DialogResult<object?>(false, null));
    }

    private static ObservingCommand BuildOkCommand(CommandBuilder<ProteusConfigDialogViewModel> cb)
    {
        var vm = cb.ViewModelReference;
        return cb.BuildObserving(() => vm.Close(new DialogResult<object?>(true, vm.GetSerializableConfig())))
            .ListensToCanExecute(o => o.IsStateValid)
            .Build();
    }
}

public abstract class ProteusConfigDialogViewModel<T> : ProteusConfigDialogViewModel where T : notnull
{
    protected abstract T GetConfig();

    protected override object? GetSerializableConfig() => GetConfig();
}
