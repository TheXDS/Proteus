using TheXDS.Ganymede.Types.Base;

namespace TheXDS.Proteus.Services.Configuration;

/// <summary>
/// Defines a ViewModel that can provide readiness information.
/// </summary>
public interface IValidatableViewModel : IViewModel
{
    /// <summary>
    /// Gets a value that indicates if the state of this ViewModel is valid.
    /// </summary>
    bool IsStateValid { get; }
}