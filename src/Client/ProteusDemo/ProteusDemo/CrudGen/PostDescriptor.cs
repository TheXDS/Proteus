using TheXDS.Proteus.Models;
using TheXDS.Proteus.ViewModels.CrudGen;

namespace TheXDS.Proteus.CrudGen;

/// <summary>
/// Describes the <see cref="Post"/> model.
/// </summary>
public class PostDescriptor : CrudDescriptor<Post>
{
    /// <inheritdoc/>
    protected override void OnDescribeModel(IModelConfigurator<Post> m)
    {
        m.ConfigureProperties(c =>
        {
            c.Property(p => p.Title);
            c.Property(p => p.Creator).Selectable();
            c.Property(p => p.Content).Paragraph(WidgetSize.Large);
            c.Property(p => p.Timestamp).Label("Created").WithTime().HideFromEditor();
            c.Property(p => p.LastUpdated).Label("Last updated").WithTime().HideFromEditor();
            c.Property(p => p.Comments)
                .HideFromDetails()
                .WidgetSize(WidgetSize.Large)
                .Creatable();
        });
        m.DetailsViewModel<PostDetailsViewModel>();
        m.AddDefaultGuidIdProlog();
        m.AddTimestampSetProlog();
        m.AddLastUpdatedProlog();
        m.ListViewProperties(p => p.Title, p => p.Timestamp, p => p.Creator, p => p.LastUpdated);
    }
}
