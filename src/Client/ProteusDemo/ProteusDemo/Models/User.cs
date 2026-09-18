#pragma warning disable CS1591

using TheXDS.Triton.Models.Base;

namespace TheXDS.Proteus.Models;

public class User : TimestampModel<string>
{
    public string? DisplayName { get; set; }
    public byte[] Password { get; set; } = [];
    public bool Enabled { get; set; } = true;
    public string? Description { get; set; } = null;
    public virtual ICollection<Post> Posts { get; set; } = [];
    public DayOfWeek FavoriteDay { get; set; }
    public LikeFlags LikeFlags { get; set; }
    public override string ToString() => DisplayName ?? Id;
}
