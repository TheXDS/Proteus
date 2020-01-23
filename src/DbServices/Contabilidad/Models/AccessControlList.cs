using System.Collections.Generic;
using TheXDS.Proteus.Models.Base;

namespace TheXDS.Proteus.Models
{
    public class AccessControlList: User<int>
    {
        public AclValue? EmpresaDefault { get; set; }
        public AclValue? EntidadDefault { get; set; }
        public AclValue? CostCenterDefault { get; set; }

        public virtual List<AclEntry> Entries { get; set; } = new List<AclEntry>();
    }
}
