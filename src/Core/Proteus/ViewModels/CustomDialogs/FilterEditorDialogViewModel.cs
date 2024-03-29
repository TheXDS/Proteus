﻿using System.Drawing;
using TheXDS.Ganymede.ViewModels;
using TheXDS.MCART.Component;
using TheXDS.MCART.Types.Extensions;
using TheXDS.Proteus.CrudGen;
using TheXDS.Proteus.Services;
using St = TheXDS.Proteus.Resources.Strings.Common;

namespace TheXDS.Proteus.ViewModels.CustomDialogs;

/// <summary>
/// Implements a custom dialog that allows the user to edit a collection of
/// <see cref="FilterItem"/>.
/// </summary>
public class FilterEditorDialogViewModel : AwaitableDialogViewModel
{
    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="FilterEditorDialogViewModel"/> class.
    /// </summary>
    /// <param name="filterCollection">
    /// Collection of filters to be managed.
    /// </param>
    public FilterEditorDialogViewModel(IDictionary<ICrudDescription, Filter> filterCollection)
    {
        FilterCollection = filterCollection.Select(p => new FilterEditorItem(p.Key, p.Value));
        Interactions.Add(new(new SimpleCommand(OnClearAll), St.ClearAndClose));
        Interactions.Add(new(new SimpleCommand(OnClose), St.ApplyAndClose));
        Title = St.Filter;
        IconBgColor = Color.Chocolate;
        Icon = "⛉";
    }

    /// <summary>
    /// Gets a reference to the filter collection being managed by this dialog.
    /// </summary>
    public IEnumerable<FilterEditorItem> FilterCollection { get; }

    private void OnClose()
    {
        foreach (var j in FilterCollection)
        {
            j.Filter.Items.RemoveAll(p => p.Property is null || p.Query.IsEmpty());
        }
        CloseDialog();
    }

    private void OnClearAll()
    {
        foreach (var j in FilterCollection)
        {
            j.Filter.Items.Clear();
        }
        CloseDialog();
    }
}
