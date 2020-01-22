using System.Collections.Generic;
using TheXDS.MCART.Types.Base;
using TheXDS.Proteus.Models.Base;

namespace TheXDS.Proteus.Models
{
    public class Partida : TimestampModel<long>, IDescriptible
    {
        public virtual Periodo Parent { get; set; }
        public string Description { get; set; }
        public virtual List<Movimiento> Movimientos { get; set; } = new List<Movimiento>();
        public virtual List<DocumentRef> Documentos { get; set; } = new List<DocumentRef>();
    }

    public class DocumentRef : ModelBase<long>
    {
        public string DocReference { get; set; }
        public virtual DocumentKind Kind { get; set; }
        public string? FilePath { get; set; }
    }

    public class DocumentKind : Nameable<byte>
    {
        public virtual List<DocumentRef> Documents { get; set; } = new List<DocumentRef>();
    }
}
