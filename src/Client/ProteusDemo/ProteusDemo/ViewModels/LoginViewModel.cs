﻿using System.Windows.Input;
using TheXDS.Ganymede.Helpers;
using TheXDS.Ganymede.Models;
using TheXDS.Ganymede.Types.Base;
using TheXDS.MCART.Helpers;
using TheXDS.MCART.Types.Extensions;
using TheXDS.Proteus.Models;
using TheXDS.Triton.Services.Base;
using SP = TheXDS.ServicePool.ServicePool;

namespace TheXDS.Proteus.ViewModels;

/// <summary>
/// ViewModel that demonstrates how a simple login screen may be implemented.
/// </summary>
public class LoginViewModel : ViewModel
{
    private string? _username;
    private string? _password;

    /// <summary>
    /// Gets or sets the username field.
    /// </summary>
    public string? Username
    {
        get => _username;
        set => Change(ref _username, value);
    }

    /// <summary>
    /// Gets or sets the password field.
    /// </summary>
    public string? Password
    {
        get => _password;
        set => Change(ref _password, value);
    }

    /// <summary>
    /// Gets a reference to the command used to log in.
    /// </summary>
    public ICommand LoginCommand { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="LoginViewModel"/> class.
    /// </summary>
    public LoginViewModel()
    {
        var cb = new CommandBuilder<LoginViewModel>(this);
        LoginCommand = cb.BuildObserving(OnLogin).CanExecuteIfFilled(p => p.Username, p => p.Password).Build();
    }

    private async Task OnLogin(IProgress<ProgressReport> progress)
    {
        progress.Report("Logging in...");
        using var trans = SP.CommonPool.Resolve<ITritonService>()!.GetReadTransaction();
        var result = await trans.ReadAsync<User, string>(Username!);
        if (result && result.Result is { } user && (PasswordStorage.VerifyPassword(Password!.ToSecureString(), user.Password) ?? false))
        {
            NavigationService!.HomePage = new WelcomeViewModel();
        }
        else
        {
            await DialogService!.Error("Could not log in", result.Reason?.ToString() ?? "Invalid username/password!");
        }
    }
}