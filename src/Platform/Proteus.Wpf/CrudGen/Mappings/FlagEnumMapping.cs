﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using TheXDS.MCART.Types.Extensions;
using TheXDS.Proteus.CrudGen.Descriptions;
using TheXDS.Proteus.CrudGen.Mappings.Base;
using TheXDS.Proteus.ViewModels;
using TheXDS.Triton.Models.Base;

namespace TheXDS.Proteus.CrudGen.Mappings;

/// <summary>
/// Maps <see cref="Enum"/> properties for non-flag enums.
/// </summary>
public class FlagEnumMapping : CrudMappingBase, ICrudMapping
{
    bool ICrudMapping.MustSetValueManually => true;

    /// <inheritdoc/>
    public bool CanMap(IPropertyDescription description)
    {
        return description.Property.PropertyType.IsEnum
            && (description is IEnumPropertyDescription e && e.Flags
            || description.Property.PropertyType.HasAttribute<FlagsAttribute>());
    }

    /// <inheritdoc/>
    public FrameworkElement CreateControl(IPropertyDescription description)
    {
        var chkMargin = new Thickness(0, 0, 5, 5);
        var bxs = new HashSet<ToggleButton>();
        var pnl = new WrapPanel
        {
            Orientation = Orientation.Vertical,
            MaxHeight = InferFlagPanelSize(description),
        };
        var groupBox = CreateGroupBox(description, pnl);
        foreach (var j in description.Property.PropertyType.AsNamedEnum())
        {
            if (j.Value.ToUnderlyingType().Equals(0)) continue;
            var chk = new CheckBox { Content = j.Name, Tag = j.Value, Margin = chkMargin };
            chk.Click += CreateCheckBoxHandler(this, description, groupBox);
            pnl.Children.Add(chk.PushInto(bxs));
        }
        groupBox.Tag = bxs;
        return groupBox;
    }

    private static GroupBox CreateGroupBox(IPropertyDescription description, FrameworkElement content)
    {
        return new GroupBox
        {
            Header = $"{description.Icon} {description.Label}",
            Padding = new Thickness(5, 5, 0, 0),
            Content = new ScrollViewer
            {
                Content = content,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                VerticalScrollBarVisibility = ScrollBarVisibility.Disabled
            }
        };
    }

    private static RoutedEventHandler CreateCheckBoxHandler(ICrudMapping mapping, IPropertyDescription description, GroupBox groupBox)
    {
        return (s, e) =>
        {
            if (s != e.OriginalSource) return;
            if (s is FrameworkElement { DataContext: CrudEditorViewModel { Entity: Model entity } } f)
            {
                description.Property.SetValue(entity, GetFlags(f, (Enum)description.Property.GetValue(entity)!, description.Property.PropertyType));
                mapping.SetControlValue(groupBox, entity, description);
            }
        };
    }

    private static double InferFlagPanelSize(IPropertyDescription description)
    {
        return description is IWidgetConfigurableDescription d && d.WidgetSize != WidgetSize.Auto ? d.WidgetSize switch
        {
            WidgetSize.Small => 90,
            WidgetSize.Medium => 120,
            WidgetSize.Large => 150,
            WidgetSize.Huge => 300,
            _ => throw new NotImplementedException(),
        } : 120;
    }

    private static Enum GetFlags(FrameworkElement control, Enum oldValue, Type enumType)
    {
        var v = ((ToggleButton)control).IsChecked ?? false;
        string newValue = ((oldValue.ToUnderlyingType(), ((Enum)control.Tag).ToUnderlyingType()) switch
        {
            (byte x, byte y) => (byte)(y != 0 ? x ^ y | (v ? y : default(int)) : 0),
            (sbyte x, sbyte y) => (sbyte)(y != 0 ? x ^ y | (v ? y : default(int)) : 0),
            (short x, short y) => (short)(y != 0 ? x ^ y | (v ? y : default(int)) : 0),
            (ushort x, ushort y) => (ushort)(y != 0 ? x ^ y | (v ? y : default(int)) : 0),
            (int x, int y) => y != 0 ? x ^ y | (v ? y : default) : 0,
            (uint x, uint y) => y != 0 ? x ^ y | (v ? y : default) : 0,
            (long x, long y) => y != 0 ? x ^ y | (v ? y : default) : 0,
            (ulong x, ulong y) => y != 0 ? x ^ y | (v ? y : default) : 0,
            _ => (object)oldValue
        }).ToString()!;
        return (Enum)Enum.Parse(enumType, newValue);
    }

    void ICrudMapping.SetControlValue(FrameworkElement control, Model entity, IPropertyDescription description)
    {
        var boxes = (IEnumerable<ToggleButton>)control.Tag;
        Enum value = (Enum)description.Property.GetValue(entity)!;
        var zero = Enum.Parse(description.Property.PropertyType, "0");
        foreach (var j in boxes)
        {
            var t = (Enum)j.Tag;
            j.IsChecked =
                t.CompareTo(zero) == 0
                ? value.CompareTo(zero) == 0
                : value.HasFlag(t);
        }
    }
}