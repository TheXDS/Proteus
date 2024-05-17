using System.Windows.Input;
using TheXDS.Ganymede.Helpers;
using TheXDS.Ganymede.Models;
using TheXDS.Ganymede.Types.Base;
using TheXDS.MCART.Helpers;
using TheXDS.MCART.Security;
using TheXDS.MCART.Types.Extensions;
using TheXDS.Proteus.Models;
using TheXDS.Triton.Faker;
using TheXDS.Triton.Services.Base;
using Sp = TheXDS.Proteus.Shared.Globals;
using St = TheXDS.Proteus.Resources.Strings.ViewModels.LoginViewModel;

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
        progress.Report(St.LoggingIn);
        if (Sp.Pool.Resolve<ITritonService>() is { } svc)
        {
            using var trans = svc.GetReadTransaction();
            var result = await trans.ReadAsync<User, string>(Username!);
            if (result && result.Result is { } user && (PasswordStorage.VerifyPassword(Password!.ToSecureString(), user.Password) ?? false))
            {
                NavigationService!.HomePage = new WelcomeViewModel();
            }
            else
            {
                await DialogService!.Error(St.CouldNotLogIn, result.Reason?.ToString() ?? St.InvalidUsernamePassword);
            }
        }
        else
        {
            await DialogService!.Warning(St.TritonServiceNotFound, St.TritonNotFoundMessage);
            NavigationService!.HomePage = new WelcomeViewModel();
        }
    }

    /// <inheritdoc/>
    protected override async Task OnCreated()
    {
        if (IsInitialized || Sp.Pool.Resolve<ITritonService>() is not { } svc) return;
        using var trans = svc.GetTransaction();
        if (trans.All<User>().Any()) return;
        var users = new[]
        {
            new User()
            {
                Id = "root",
                DisplayName = "Super User",
                Enabled = false,
                Password = PasswordStorage.CreateHash<Pbkdf2Storage>("r00t".ToSecureString())
            },
            new User()
            {
                Id = "admin",
                DisplayName = "Administrator",
                Password = PasswordStorage.CreateHash<Pbkdf2Storage>("@dmin1234".ToSecureString())
            }
        }.Concat(Enumerable.Range(0, 10).Select(_ => Person.Adult()).Select(p => new User()
        {
            Id = p.UserName,
            DisplayName = p.Name,
            Password = PasswordStorage.CreateHash<Pbkdf2Storage>("1234".ToSecureString())
        })).ToArray();

        trans.Create(users);
        var admPost = new Post()
        {
            Title = Text.Lorem(4),
            Content = Text.Lorem(200, 8, 3),
            CreationDate = DateTime.Now,
            Creator = users[1],
            Id = Guid.NewGuid(),
        };
        var comments = users[2..].Select(u => new Comment()
        {
            Id = Guid.NewGuid(),
            Content = Text.Lorem(10),
            CreationDate = DateTime.Now,
            Creator = u,
            Post = admPost
        });
        admPost.Comments = comments.ToList();
        trans.Create(admPost);
        trans.Create(comments.ToArray());
        await trans.CommitAsync();
    }
}