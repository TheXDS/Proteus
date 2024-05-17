using System.Windows.Input;
using TheXDS.Ganymede.Helpers;
using TheXDS.Ganymede.Types.Base;
using TheXDS.Proteus.CrudGen;
using TheXDS.Proteus.Services;
using TheXDS.Triton.Services.Base;
using Sp = TheXDS.Proteus.Shared.Globals;

namespace TheXDS.Proteus.ViewModels;

/// <summary>
/// Implements a ViewModel that includes a few demos for Proteus technology.
/// </summary>
public class ProteusDemoViewModel : ViewModel
{
    /// <summary>
    /// Gets a command that manages users.
    /// </summary>
    public ICommand ManageUsersCommand { get; }

    /// <summary>
    /// Gets a command that manages posts.
    /// </summary>
    public ICommand ManagePostsCommand { get; }

    /// <summary>
    /// Gets a command that manages comments.
    /// </summary>
    public ICommand ManageCommentsCommand { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProteusDemoViewModel"/>
    /// class.
    /// </summary>
    public ProteusDemoViewModel()
    {
        CommandBuilder<ProteusDemoViewModel> cb = new(this);
        ManageUsersCommand = cb.BuildSimple(OnManage<UserDescriptor>);
        ManagePostsCommand = cb.BuildSimple(OnManage<PostDescriptor>);
        ManageCommentsCommand = cb.BuildSimple(OnManage<CommentDescriptor>);
    }

    private void OnManage<TDescriptor>() where TDescriptor : ICrudDescriptor, new()
    {
        var svc = Sp.Pool.Resolve<ITritonService>()!;
        var ep = new TritonFlatEntityProvider(svc, new TDescriptor().Description);
        var vm = new CrudPageViewModel([new TDescriptor().Description], svc, ep) { DialogService = DialogService };
        NavigationService?.Navigate(vm);
    }
}
