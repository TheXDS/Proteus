using TheXDS.Triton.Models.Base;

namespace TheXDS.Proteus.CrudGen;

/// <summary>
/// Includes a set of extensions for the <see cref="IModelConfigurator{T}"/>
/// interface.
/// </summary>
public static class ModelConfiguratorExtensions
{
    /// <summary>
    /// Adds a save prolog to a model description that will automatically set a
    /// unique ID for any new entity where the ID field is of type
    /// <see cref="Guid"/>.
    /// </summary>
    /// <typeparam name="T">Model type.</typeparam>
    /// <param name="configurator">
    /// Configurator to add the desired save prolog onto.
    /// </param>
    /// <returns>
    /// The same <see cref="IModelConfigurator{T}"/> instance as
    /// <paramref name="configurator"/>, allowing the use of Fluent syntax.
    /// </returns>
    public static IModelConfigurator<T> AddDefaultGuidIdProlog<T>(this IModelConfigurator<T> configurator) where T : Model<Guid>
    {
        return configurator.AddSaveProlog(p => { if (p.Id == default) p.Id = Guid.NewGuid(); });
    }

    /// <summary>
    /// Adds a save prolog to a model description that will automatically set a
    /// unique ID for any new entity where the ID field is of type
    /// <see cref="string"/>.
    /// </summary>
    /// <typeparam name="T">Model type.</typeparam>
    /// <param name="configurator">
    /// Configurator to add the desired save prolog onto.
    /// </param>
    /// <returns>
    /// The same <see cref="IModelConfigurator{T}"/> instance as
    /// <paramref name="configurator"/>, allowing the use of Fluent syntax.
    /// </returns>
    public static IModelConfigurator<T> AddDefaultStringIdProlog<T>(this IModelConfigurator<T> configurator) where T : Model<string>
    {
        return configurator.AddDefaultStringIdProlog(_ => Guid.NewGuid().ToString());
    }

    /// <summary>
    /// Adds a save prolog to a model description that will automatically set a
    /// unique ID for any new entity where the ID field is of type
    /// <see cref="string"/>.
    /// </summary>
    /// <typeparam name="T">Model type.</typeparam>
    /// <param name="configurator">
    /// Configurator to add the desired save prolog onto.
    /// </param>
    /// <param name="idGenerator">
    /// Method to invoke when trying to generate a string ID.
    /// </param>
    /// <returns>
    /// The same <see cref="IModelConfigurator{T}"/> instance as
    /// <paramref name="configurator"/>, allowing the use of Fluent syntax.
    /// </returns>
    public static IModelConfigurator<T> AddDefaultStringIdProlog<T>(this IModelConfigurator<T> configurator, Func<T, string> idGenerator) where T : Model<string>
    {
        return configurator.AddSaveProlog(p => { if (string.IsNullOrEmpty(p.Id)) p.Id = idGenerator.Invoke(p); });
    }

    /// <summary>
    /// Adds a prolog action to the model configurator that updates the model's timestamp to the current date and time
    /// before saving.
    /// </summary>
    /// <remarks>This method is typically used to ensure that the model's timestamp is automatically set to
    /// the current time when the model is created.</remarks>
    /// <typeparam name="T">The type of the model being configured. Must inherit from Model and implement ITimestampModel.</typeparam>
    /// <param name="configurator">The model configurator to which the timestamp update prolog will be added.</param>
    /// <returns>The same model configurator instance with the timestamp update prolog configured.</returns>
    public static IModelConfigurator<T> AddTimestampSetProlog<T>(this IModelConfigurator<T> configurator) where T : Model, ITimestampModel
    {
        return configurator.AddSaveProlog(p => p.Timestamp ??= DateTime.Now);
    }


    /// <summary>
    /// Adds a prolog action to the model configurator that updates the model's timestamp to the current date and time
    /// before saving.
    /// </summary>
    /// <remarks>This method is typically used to ensure that the model's timestamp is automatically set to
    /// the current time when the model is created.</remarks>
    /// <typeparam name="T">The type of the model being configured. Must inherit from Model and implement ITimestampModel.</typeparam>
    /// <param name="configurator">The model configurator to which the timestamp update prolog will be added.</param>
    /// <returns>The same model configurator instance with the timestamp update prolog configured.</returns>
    public static IModelConfigurator<T> AddLastUpdatedProlog<T>(this IModelConfigurator<T> configurator) where T : Model, ILastUpdatedModel
    {
        return configurator.AddSaveProlog(p => p.LastUpdated = DateTime.Now);
    }
}
