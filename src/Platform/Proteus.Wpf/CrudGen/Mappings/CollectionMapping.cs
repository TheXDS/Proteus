using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Input;
using TheXDS.MCART.Component;
using TheXDS.MCART.Types.Extensions;
using TheXDS.Proteus.Controls;
using TheXDS.Proteus.CrudGen.Descriptions;
using TheXDS.Proteus.CrudGen.Mappings.Base;
using TheXDS.Proteus.Helpers;
using TheXDS.Proteus.ViewModels;
using TheXDS.Triton.Models.Base;
using St = TheXDS.Proteus.Resources.Strings.Common;

namespace TheXDS.Proteus.CrudGen.Mappings;

/// <summary>
/// Implements a mapping that generates controls for managing collections of
/// entities.
/// </summary>
public class CollectionMapping : ObjectMappingBase<ListEditor, ICollectionPropertyDescription>
{
    /// <inheritdoc/>
    protected override void ConfigureControl(ListEditor control, ICollectionPropertyDescription description)
    {
        if (description.WidgetSize != WidgetSize.Auto)
        {
            control.Height = description.WidgetSize switch
            {
                WidgetSize.Small => 90,
                WidgetSize.Medium => 150,
                WidgetSize.Large => 250,
                WidgetSize.Huge => 500,
                _ => 0
            };
        }
        control.RemoveCommand = CreateRemoveCommand(control, description);
    }

    private static ICommand CreateRemoveCommand(ListEditor control, ICollectionPropertyDescription description)
    {
        return new SimpleCommand(async () =>
        {
            var vm = (CrudEditorViewModel)control.DataContext;
            if (control.SelectedEntity is not { } child || description.Property.GetValue(vm.Entity) is not { } parentCollection) return;
            if (await vm.DialogService!.Ask(St.Delete, St.AreYouSureDelete))
            {
                CrudCommon.DynamicRemove(parentCollection, child);
                control.Collection.Remove(child);
                control.InvalidateProperty(ListEditor.CollectionProperty);
            }
        });
    }

    /// <inheritdoc/>
    public override void SetControlValue(ListEditor control, object? propertyValue)
    {
        var controlCollection = new List<Model>();
        foreach (Model j in (IEnumerable<Model>?)propertyValue ?? Array.Empty<Model>())
        {
            controlCollection.Add(j);
        }
        control.Collection = controlCollection;
    }

    /// <inheritdoc/>
    protected override void OnAddNew(ListEditor control, Model newEntity, PropertyInfo parentProperty, Model parentEntity)
    {
        object? targetCollection = parentProperty.GetValue(parentEntity);
        if (targetCollection is not null)
        {
            CrudCommon.DynamicAdd(targetCollection, newEntity);
        }
        else
        {
            var itemType = parentProperty.PropertyType.GetCollectionType();
            var list = typeof(List<>).MakeGenericType(itemType).New();
            CrudCommon.DynamicAdd(list, newEntity);
            parentProperty.SetValue(parentEntity, list);
        }
    }
}
