using System.Collections.Generic;
using TheXDS.Proteus.Models.Base;

namespace TheXDS.Proteus.Models
{
    public class Entidad : Nameable<int>
    {
        public virtual List<Periodo> Periodos { get; set; } = new List<Periodo>();

        public virtual Cuenta Activo { get; set; }
        public virtual Cuenta Pasivo { get; set; }
        public virtual Cuenta Capital { get; set; }
    }
}
