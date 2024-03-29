﻿using TheXDS.Proteus.CrudGen.Descriptors;
using TheXDS.Triton.Models.Base;

namespace TheXDS.Proteus.CrudGen.Descriptions;

/// <summary>
/// Defines an <see cref="IPropertyDescriptor"/> used to describe properties
/// that can store single instances of <c><see cref="Model"/></c>.
/// </summary>
public interface ISingleObjectPropertyDescription : IObjectPropertyDescription, INullablePropertyDescription
{
}