﻿using System.Linq.Expressions;
using System.Reflection;
using TheXDS.MCART.Helpers;
using TheXDS.MCART.Types.Extensions;
using TheXDS.Proteus.Helpers;
using TheXDS.Triton.Models.Base;

namespace TheXDS.Proteus.CrudGen;

/// <summary>
/// Implements the model configuration logic for CRUD generation.
/// </summary>
/// <typeparam name="T">Model to configure.</typeparam>
public class CrudDescriptorConfigurator<T> : ICrudDescription, IModelConfigurator<T> where T : Model
{
    private readonly Dictionary<PropertyInfo, DescriptionEntry> _properties;
    private readonly IPropertyConfigurator<T> _propertyConfigurator;
    private readonly List<Action<Model>> _savePrologs = [];
    private readonly List<PropertyInfo> _listViewProps = [];

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="CrudDescriptorConfigurator{T}"/> class.
    /// </summary>
    public CrudDescriptorConfigurator()
    {
        FriendlyName = ResourceType.GetLabel($"{typeof(T).Name}{(ResourceType is not null && ResourceType.Name == typeof(T).Name ? "Model" : null)}");
        _properties = [];
        _propertyConfigurator = new PropertyDescriptorConfigurator<T>(_properties, this);
    }

    /// <inheritdoc/>
    public IReadOnlyDictionary<PropertyInfo, DescriptionEntry> PropertyDescriptions => _properties;

    /// <inheritdoc/>
    public Type Model => typeof(T);

    /// <inheritdoc/>
    public string? FriendlyNameBindingPath { get; private set; }

    /// <inheritdoc/>
    public string FriendlyName { get; private set; }

    /// <inheritdoc/>
    public IEnumerable<Action<Model>> SavePrologs => _savePrologs;

    /// <inheritdoc/>
    public Type? DashboardViewModel { get; private set; }

    /// <inheritdoc/>
    public Type? DetailsViewModel { get; private set; }

    /// <inheritdoc/>
    public Type? ResourceType { get; private set; }

    /// <inheritdoc/>
    public IEnumerable<PropertyInfo> ListViewProperties => _listViewProps.Count != 0 ? _listViewProps : InferListViewProps();

    /// <inheritdoc/>
    public CrudCategory? Category { get; private set; }

    private IEnumerable<PropertyInfo> InferListViewProps()
    {
        return PropertyDescriptions
            .Where(p => !p.Value.Description.HideFromDetails)
            .Select(p => p.Key);
    }

    IModelConfigurator<T> IModelConfigurator<T>.FriendlyName(string friendlyName)
    {
        FriendlyName = friendlyName;
        return this;
    }

    IModelConfigurator<T> IModelConfigurator<T>.FriendlyNameBindingPath(string propertyPath)
    {
        FriendlyNameBindingPath = propertyPath;
        return this;
    }

    IModelConfigurator<T> IModelConfigurator<T>.AddSaveProlog(Action<T> prolog)
    {
        _savePrologs.Add(m => prolog.Invoke((T)m));
        return this;
    }

    IModelConfigurator<T> IModelConfigurator<T>.ConfigureProperties(Action<IPropertyConfigurator<T>> config)
    {
        config.Invoke(_propertyConfigurator);
        return this;
    }

    IModelConfigurator<T> IModelConfigurator<T>.LabelResource<TRes>()
    {
        ResourceType = typeof(TRes);
        return this;
    }

    IModelConfigurator<T> IModelConfigurator<T>.DashboardViewModel<TViewModel>()
    {
        DashboardViewModel = typeof(TViewModel);
        return this;
    }

    IModelConfigurator<T> IModelConfigurator<T>.DetailsViewModel<TViewModel>()
    {
        DetailsViewModel = typeof(TViewModel);
        return this;
    }

    IModelConfigurator<T> IModelConfigurator<T>.ListViewProperties(params Expression<Func<T, object?>>[] propertySelectors)
    {
        _listViewProps.AddRange(propertySelectors.Select(ReflectionHelpers.GetProperty));
        return this;
    }

    IModelConfigurator<T> IModelConfigurator<T>.Category(CrudCategory category)
    {
        Category = category;
        return this;
    }
}
