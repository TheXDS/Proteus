using TheXDS.Proteus.Models;
using St = TheXDS.Proteus.Resources.Strings.Models;

namespace TheXDS.Proteus.CrudGen;

/// <summary>
/// Describes the <see cref="User"/> model.
/// </summary>
public class UserDescriptor : CrudDescriptor<User>
{
    /// <inheritdoc/>
    protected override void OnDescribeModel(IModelConfigurator<User> m)
    {
        m.Category(CrudCategory.Settings);
        m.LabelResource<St.User>();
        m.ConfigureProperties(c =>
        {
            c.Property(p => p.Id);
            c.Property(p => p.DisplayName).Icon("👤");
            c.Property(p => p.Password).Password().Argon2();
            c.Property(p => p.Enabled);
            c.Property(p => p.Description).Nullable().Kind(TextKind.Paragraph);
            c.Property(p => p.FavoriteDay);
            c.Property(p => p.LikeFlags);
        });
        m.ListViewProperties(p => p.Id, p => p.DisplayName);
    }
}
