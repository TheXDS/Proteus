using System.Windows;
using TheXDS.Proteus.CrudGen.Descriptions;
using TheXDS.Proteus.CrudGen.Mappings.Base;

namespace TheXDS.Proteus.CrudGen.Mappings;

/// <summary>
/// Base class that defines a simple CRUD mapping between a property type and a
/// <see cref="FrameworkElement"/>.
/// </summary>
/// <typeparam name="TProp">Property type to map.</typeparam>
/// <typeparam name="TControl">
/// Type of <see cref="FrameworkElement"/> to generate.
/// </typeparam>
public abstract class SimpleCrudMappingBase<TProp, TControl> : SimpleCrudMappingBase<TControl>, ICrudMapping
    where TControl : FrameworkElement, new()
{
    /// <inheritdoc/>
    public override bool CanMap(IPropertyDescription description)
    {
        return description.Property.PropertyType == typeof(TProp);
    }
}
