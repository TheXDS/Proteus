#pragma warning disable CS1591

using TheXDS.Triton.Models.Base;

namespace TheXDS.Proteus.Models;

public class Comment : TimestampModel<Guid>
{
    public User? Creator { get; set; }
    public Post? Post { get; set; }
    public string? Content { get; set; }
    public override string ToString() => Content ?? Id.ToString();
}
