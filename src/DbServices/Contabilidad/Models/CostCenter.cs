using System.Collections.Generic;
using TheXDS.Proteus.Models.Base;

namespace TheXDS.Proteus.Models
{
    public class CostCenter : Nameable<int>
    {
        public virtual Entidad Parent { get; set; }
        public virtual List<Movimiento> Movimientos { get; set; } = new List<Movimiento>();
    }
}
