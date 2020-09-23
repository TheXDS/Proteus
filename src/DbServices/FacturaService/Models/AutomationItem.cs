using System;
using System.Linq;
using TheXDS.MCART.Types.Extensions;
using TheXDS.Proteus.Api;
using TheXDS.Proteus.Component;
using TheXDS.Proteus.Models.Base;

namespace TheXDS.Proteus.Models
{
    public class AutomationItem : ModelBase<long>
    {
        public Guid Automator { get; set; }
        public IFacturableAutomation? ResolveAutomator()
        {
            return FacturaService.Automators.FirstOrDefault(p => p.Guid == Automator);
        }
        public override string ToString()
        {
            return ResolveAutomator()?.GetType().NameOf() ?? "n/a";
        }
    }
}