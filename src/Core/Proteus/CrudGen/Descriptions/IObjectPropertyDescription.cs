﻿using TheXDS.MCART.Types.Extensions;
using TheXDS.Proteus.CrudGen.Descriptors;
using TheXDS.Proteus.Helpers;
using TheXDS.Triton.Models.Base;

namespace TheXDS.Proteus.CrudGen.Descriptions;

/// <summary>
/// Defines an <see cref="IPropertyDescriptor"/> used to describe properties
/// that store <c><see cref="Model"/></c>, either as a single element or as
/// part of a collection.
/// </summary>
public interface IObjectPropertyDescription : IPropertyDescription
{
    /// <summary>
    /// Indicates that a property must support adding existing entities.
    /// </summary>
    bool Selectable => GetStructValue<bool>() ?? false;

    /// <summary>
    /// Indicates that a property must support creating new entities. This
    /// also implies the ability to update existing items.
    /// </summary>
    bool Creatable => GetStructValue<bool>() ?? false;

    /// <summary>
    /// Indicates the available models to be added/selected/updated.
    /// </summary>
    ICrudDescription[] AvailableModels => (GetClassValue<ICrudDescription[]>()?.OrNull() ?? CrudCommon.InferDescriptions(this)).ToArray();
}
