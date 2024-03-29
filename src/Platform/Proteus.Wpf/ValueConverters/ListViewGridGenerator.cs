﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using TheXDS.MCART.ValueConverters.Base;
using TheXDS.Proteus.CrudGen;
using TheXDS.Proteus.CrudGen.Descriptions;

namespace TheXDS.Proteus.ValueConverters;

/// <summary>
/// Converter that generates <see cref="GridView"/> objects to customize
/// <see cref="ListView"/> presentation.
/// </summary>
public class ListViewGridGenerator : IOneWayValueConverter<ICrudDescription[]?, ViewBase?>
{
    /// <inheritdoc/>
    public ViewBase? Convert(ICrudDescription[]? value, object? parameter, CultureInfo? culture)
    {
        if (value is null) return null;
        var view = new GridView();
        foreach (var j in GetListDescriptions(value))
        {
            view.Columns.Add(CreateColumn(j));
        }
        return view;
    }

    private static IEnumerable<IPropertyDescription> GetListDescriptions(ICrudDescription[] descriptions)
    {
        var props = descriptions.SelectMany(p => p.ListViewProperties).ToArray();
        return descriptions.SelectMany(p => p.PropertyDescriptions).Where(p => props.Contains(p.Key)).Select(p => p.Value.Description);
    }

    private static GridViewColumn CreateColumn(IPropertyDescription p)
    {
        return new GridViewColumn()
        {
            Header = p.Label,
            DisplayMemberBinding = new Binding(p.Property.Name)
        };
    }
}
