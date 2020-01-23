using TheXDS.MCART.Attributes;

namespace TheXDS.Proteus.Models
{
    public enum AclValue : byte
    {
        [Name("Permitir")] Allow,
        [Name("Denegar")] Deny
    }
}
