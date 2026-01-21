using TheXDS.Proteus.Models;

namespace TheXDS.Proteus.CrudGen;

/// <summary>
/// Describes the <see cref="Comment"/> model.
/// </summary>
public class CommentDescriptor : CrudDescriptor<Comment>
{
    /// <inheritdoc/>
    protected override void OnDescribeModel(IModelConfigurator<Comment> m)
    {
        m.ConfigureProperties(c =>
        {
            c.Property(p => p.Post).Selectable();
            c.Property(p => p.Creator).Selectable();
            c.Property(p => p.Content).Paragraph();
            c.Property(p => p.Timestamp).Label("Creation date").WithTime().HideFromEditor();
        });
        m.AddDefaultGuidIdProlog();
        m.AddTimestampSetProlog();
        m.ListViewProperties(p => p.Timestamp, p => p.Creator);
    }
}