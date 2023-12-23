using Microsoft.EntityFrameworkCore;
using TheXDS.MCART.Types.Extensions;
using TheXDS.ServicePool.Triton;
using TheXDS.Triton.Services;
using TheXDS.Triton.Services.Base;

namespace TheXDS.Proteus.Services.Configuration;

/// <summary>
/// Implements a service configurator that configures and sets a new
/// <typeparamref name="TService"/> instance connected to Entity Framework as
/// the Triton service to use.
/// </summary>
public abstract class TritonEfConfiguratorBase<TService, TContext> : ITritonServiceConfigurator where TContext : DbContext where TService : ITritonService
{
    /// <inheritdoc/>
    public virtual string DisplayName
    {
        get
        {
            var str = string.Join(" ", GetType().Name.ChopEnd("Configurator").SplitByCase());
            return str[0].ToString().ToUpper() + str[1..].ToLower();
        }
    }

    /// <inheritdoc/>
    public virtual Guid ClassId
    {
        get
        {
            return new(System.Text.Encoding.UTF8.GetBytes(GetType().Name).Concat(new byte[16]).ToArray()[0..16]);
        }
    }

    /// <inheritdoc/>
    public virtual IValidatableViewModel? ConfigurationViewModel => null;
    
    /// <summary>
    /// Creates a new instance of the required service, given a set of
    /// configuration values.
    /// </summary>
    /// <param name="middlewareConfigurator">
    /// Middleware configurator to use.
    /// </param>
    /// <param name="transactionFactory">Transaction factory to use.</param>
    /// <returns>
    /// A new instance of the <typeparamref name="TService"/> type.
    /// </returns>
    protected abstract TService CreateService(IMiddlewareConfigurator middlewareConfigurator, EfCoreTransFactory<TContext> transactionFactory);

    /// <summary>
    /// Configures the options to be passed onto a <see cref="DbContext"/>.
    /// </summary>
    /// <param name="builder">
    /// Options builder to use when configuring the <see cref="DbContext"/>
    /// options.
    /// </param>
    protected abstract void ConfigureContext(DbContextOptionsBuilder<TContext> builder);

    void ITritonServiceConfigurator.Configure(ITritonConfigurable configurable)
    {
        configurable.UseService<TService, TContext>(CreateService, ConfigureContext);
    }
}
