using System.Collections.Generic;
using System.Linq;
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
        public virtual Entidad? Entidad { get; set; }
    }
}