using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using TheXDS.MCART.Types.Extensions;
using TheXDS.Proteus.CrudGen.Descriptions;
using TheXDS.Proteus.CrudGen.Mappings.Base;

namespace TheXDS.Proteus.CrudGen.Mappings;

/// <summary>
/// Maps <see cref="Enum"/> properties for non-flag enums.
/// </summary>
public class EnumMapping : CrudMappingBase, ICrudMapping
{
    /// <inheritdoc/>
    public bool CanMap(IPropertyDescription description)
    {
        return description.Property.PropertyType.IsEnum
            && ((description is IEnumPropertyDescription e && !e.Flags)
            || !description.Property.PropertyType.HasAttribute<FlagsAttribute>());
    }

    /// <inheritdoc/>
    public FrameworkElement CreateControl(IPropertyDescription description)
    {
        var c = new ComboBox
        {
            ItemsSource = description.Property.PropertyType.AsNamedEnum(),
            SelectedValuePath = "Value",
            DisplayMemberPath = "Name"
        };
        c.SetBinding(Selector.SelectedValueProperty, description.GetBindingString());
        return c;
    }
}
