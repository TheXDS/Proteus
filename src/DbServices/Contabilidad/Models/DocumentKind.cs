using System.Collections.Generic;
using TheXDS.Proteus.Models.Base;

namespace TheXDS.Proteus.Models
{
    public class DocumentKind : Nameable<int>
    {
        public virtual List<DocumentRef> Documents { get; set; } = new List<DocumentRef>();
    }
}
