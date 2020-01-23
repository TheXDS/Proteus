using System.Collections.Generic;
using TheXDS.Proteus.Models.Base;

namespace TheXDS.Proteus.Models
{
    public class Entidad : Nameable<int>
    {
        public virtual Empresa Parent { get; set; }
        public virtual List<CostCenter> CostCenters { get; set; } = new List<CostCenter>();
        public virtual List<Partida> Partidas { get; set; } = new List<Partida>();
    }
}
