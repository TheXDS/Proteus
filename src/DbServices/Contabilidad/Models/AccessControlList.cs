using System.Collections.Generic;
using TheXDS.MCART.Types.Extensions;
using TheXDS.Proteus.Models.Base;

namespace TheXDS.Proteus.Models
{
    public class AccessControlList: User<int>
    {
        public AclValue? EmpresaDefault { get; set; }
        public AclValue? EntidadDefault { get; set; }
        public AclValue? CostCenterDefault { get; set; }

        public virtual List<AclEntry> Entries { get; set; } = new List<AclEntry>();

        public override string ToString()
        {
            return $"Control de acceso{Proteus.ResolveLink<User>(UserId)?.Name?.OrNull(" para {0}")}";
        }
    }
}
