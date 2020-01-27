using System.Collections.Generic;
using TheXDS.Proteus.Models.Base;

namespace TheXDS.Proteus.Models
{
    public class DocumentKind : Nameable<short>
    {
        public byte Prefix { get; set; }
        public virtual List<DocumentRef> Documents { get; set; } = new List<DocumentRef>();
    }
}
