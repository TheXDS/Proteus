using System.Collections.Generic;
using TheXDS.Proteus.Models.Base;

namespace TheXDS.Proteus.Models
{
    public class Empresa : Addressable<int>
    {
        public string? RTN { get; set; }
        public virtual List<Periodo> Periodos { get; set; } = new List<Periodo>();

        public virtual Cuenta Activo { get; set; }
        public virtual Cuenta Pasivo { get; set; }
        public virtual Cuenta Patrimonio { get; set; }
        public virtual List<Entidad> Entidades { get; set; } = new List<Entidad>();
    }
}
