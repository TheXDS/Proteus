﻿using TheXDS.Proteus.CrudGen.Descriptors;
using TheXDS.Triton.Models.Base;

namespace TheXDS.Proteus.CrudGen.Descriptions;

/// <summary>
/// Defines an <see cref="IPropertyDescriptor"/> used to describe properties
/// that store <c><see cref="Model"/></c> collections.
/// </summary>
public interface ICollectionPropertyDescription : IObjectPropertyDescription, IWidgetConfigurableDescription
{
}
