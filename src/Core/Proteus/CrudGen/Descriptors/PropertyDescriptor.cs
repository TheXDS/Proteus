﻿using System.Reflection;
using System.Runtime.CompilerServices;
using TheXDS.Proteus.CrudGen.Descriptions;
using TheXDS.Proteus.Helpers;

namespace TheXDS.Proteus.CrudGen.Descriptors;

/// <summary>
/// Implements the basic functionality of a property descriptor class.
/// </summary>
public class PropertyDescriptor : IPropertyDescriptor, IPropertyDescription
{
    private readonly Dictionary<string, object?> values = new();

    /// <summary>
    /// Gets a value from this property descriptor.
    /// </summary>
    /// <param name="name">Name of the value to get.</param>
    /// <returns>
    /// The requested property description value, or <see langword="null"/> if
    /// the requested property description value has not been set.
    /// </returns>
    public object? GetValue([CallerMemberName] string name = null!) => values.TryGetValue(name, out object? value) ? value : null;

    /// <summary>
    /// Sets a value on this property descriptor.
    /// </summary>
    /// <param name="value">Value to set.</param>
    /// <param name="name">Name of the value to set.</param>
    public void SetValue(object? value, [CallerMemberName] string name = null!)
    {
        if (!values.ContainsKey(name)) values.Add(name, value);
        else values[name] = value;
    }

    /// <inheritdoc/>
    public PropertyInfo Property { get; init; } = null!;

    /// <inheritdoc/>
    public ICrudDescription Parent { get; init; } = null!;

    /// <inheritdoc/>
    public string Label => Parent.ResourceType.GetLabel(((IPropertyDescription)this).GetClassValue<string>() ?? Property.Name);

    /// <inheritdoc/>
    public string? Icon => ((IPropertyDescription)this).GetClassValue<string>();

    /// <inheritdoc/>
    public bool ReadOnly => ((IPropertyDescription)this).GetStructValue<bool>() ?? !Property.CanWrite;
}

/// <summary>
/// Implements the basic functionality of a property descriptor class.
/// </summary>
/// <typeparam name="T">Type of the described property.</typeparam>
public class PropertyDescriptor<T> : PropertyDescriptor, IPropertyDescriptor<T>
{
}