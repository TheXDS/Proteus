using System.Windows;
using TheXDS.Ganymede.Component;
using TheXDS.Proteus.CrudGen.Descriptions;
using TheXDS.Proteus.ViewModels;
using TheXDS.Proteus.CrudGen.Mappings;

namespace TheXDS.Proteus.Component;

/// <summary>
/// Implements a <see cref="IVisualResolver{TVisual}"/> that dynamically
/// generates Visuals for instances of <see cref="CrudDetailsViewModel"/>.
/// </summary>
public class CrudDetailsVisualBuilder : CrudVisualBuilderBase<CrudDetailsViewModel>
{
    private static readonly ReadOnlyMapping _roMapping = new();

    /// <inheritdoc/>
    protected override FrameworkElement? GetControl(IPropertyDescription description, CrudDetailsViewModel _)
    {
        return description.HideFromDetails ? null : _roMapping.CreateControl(description);
    }
}
