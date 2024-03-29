﻿using System;
using System.Windows.Controls;
using TheXDS.Proteus.CrudGen.Descriptions;

namespace TheXDS.Proteus.CrudGen.Mappings;

/// <summary>
/// Maps <see cref="DateTime"/> properties to a <see cref="DatePicker"/>.
/// </summary>
public class DateTimeMapping : SimpleCrudMapping<DateTime, DatePicker>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DateTimeMapping"/>
    /// class.
    /// </summary>
    public DateTimeMapping() : base(DatePicker.SelectedDateProperty)
    {
    }

    /// <inheritdoc/>
    protected override void ConfigureControl(DatePicker picker, IPropertyDescription description)
    {
        if (description is not INumericPropertyDescription<DateTime> d) return;

        picker.DisplayDateEnd = d.ValidRange?.Maximum;
        picker.DisplayDateStart = d.ValidRange?.Minimum;
    }
}