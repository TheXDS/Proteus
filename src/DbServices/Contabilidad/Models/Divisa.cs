using System.Collections.Generic;
using TheXDS.Proteus.Models.Base;

namespace TheXDS.Proteus.Models
{
    public class Divisa : Nameable<string>
    {
        public string Symbol { get; set; }
        public virtual List<SubCuenta> Movimientos { get; set; } = new List<SubCuenta>();

    }
}