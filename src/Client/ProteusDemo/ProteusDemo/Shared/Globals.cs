using TheXDS.ServicePool;

namespace TheXDS.Proteus.Shared;

/// <summary>
/// Contains a static collection of globally-available singletons to be used
/// during the lifetime of the application.
/// </summary>
public static class Globals
{
    /// <summary>
    /// Gets a reference to the global service pool for this application.
    /// </summary>
    public static Pool Pool { get; } = new Pool(PoolConfig.FlexResolve);
}
