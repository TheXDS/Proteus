using TheXDS.ServicePool.Triton;

namespace TheXDS.Proteus.Services.Configuration;

/// <summary>
/// Defines a set of members to be implemented by a type that allows for Tritón
/// configuration.
/// </summary>
public interface ITritonServiceConfigurator
{
    /// <summary>
    /// Gets a name to be displayed on the UI for this configurator instance.
    /// </summary>
    string DisplayName { get; }

    /// <summary>
    /// Gets a <see cref="Guid"/> that uniquely identifies this configurator
    /// type.
    /// </summary>
    Guid ClassId { get; }

    /// <summary>
    /// Executes configuration steps on the <see cref="ITritonConfigurable"/>
    /// instance.
    /// </summary>
    /// <param name="configurable">
    /// Object instance used to configure Triton.
    /// </param>
    void Configure(ITritonConfigurable configurable);

    /// <summary>
    /// Gets a reference to a <see cref="IValidatableViewModel"/> used to
    /// configure the service on this instance.
    /// </summary>
    IValidatableViewModel? ConfigurationViewModel { get; }
}
