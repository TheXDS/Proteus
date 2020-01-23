using TheXDS.Proteus.Models.Base;

namespace TheXDS.Proteus.Models
{
    public class DocumentRef : ModelBase<long>
    {
        public virtual DocumentKind Kind { get; set; }
        public string DocReference { get; set; }
        public string? FilePath { get; set; }
    }
}
