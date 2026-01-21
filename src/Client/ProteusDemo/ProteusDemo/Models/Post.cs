#pragma warning disable CS1591

using TheXDS.Triton.Models.Base;

namespace TheXDS.Proteus.Models;

public class Post : TimestampModel<Guid>, ILastUpdatedModel
{
    public User? Creator { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public virtual ICollection<Comment> Comments { get; set; } = [];
    public DateTime LastUpdated { get; set; }
    public override string ToString() => Title ?? Id.ToString();
}
