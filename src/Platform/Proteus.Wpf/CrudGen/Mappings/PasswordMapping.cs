using System.Windows;
using System.Windows.Controls;
using TheXDS.Ganymede.Controls;
using TheXDS.MCART.Helpers;
using TheXDS.Proteus.CrudGen.Descriptions;
using TheXDS.Proteus.CrudGen.Mappings.Base;
using TheXDS.Proteus.ViewModels;
using TheXDS.Triton.Models.Base;

namespace TheXDS.Proteus.CrudGen.Mappings;

/// <summary>
/// Maps password storage fields to a <see cref="PasswordBox"/>.
/// </summary>
public class PasswordMapping : CrudMappingBase, ICrudMapping<IPasswordPropertyDescription>
{
    /// <inheritdoc/>
    public FrameworkElement CreateControl(IPasswordPropertyDescription description)
    {
        var c = new PasswordBox();
        ExtraProps.SetLabel(c, description.Label);
        c.PasswordChanged += (s, e) =>
        {
            if (c.DataContext is CrudEditorViewModel { Entity: Model entity })
            {
                description.Property.SetValue(entity, PasswordStorage.CreateHash(description.Algorithm, c.SecurePassword));
            }
        };
        return c;
    }
}
