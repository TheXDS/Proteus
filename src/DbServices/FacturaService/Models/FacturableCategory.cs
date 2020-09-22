using System.Collections.Generic;
using TheXDS.Proteus.Models.Base;

namespace TheXDS.Proteus.Models
{
    public class FacturableCategory : FacturableBase<int>
    {
        public virtual List<Facturable> Children { get; set; } = new List<Facturable>();
    }
}