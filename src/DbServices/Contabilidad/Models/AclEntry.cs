using TheXDS.Proteus.Models.Base;

namespace TheXDS.Proteus.Models
{
    public class AclEntry: ModelBase<int>
    {
        public Empresa? Empresa { get; set; }
        public Entidad? Entidad { get; set; }
        public CostCenter? CostCenter { get; set; }
        public AclValue Value { get; set; }

    }
}
