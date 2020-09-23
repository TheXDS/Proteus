using System;
using System.Collections.Generic;
using System.Linq;
using TheXDS.MCART.Types.Extensions;
using TheXDS.Proteus.Api;
using TheXDS.Proteus.Models.Base;

namespace TheXDS.Proteus.Models.Misc
{
    public class AutomationItemSource : ModelBase<int>
    {
        public static IQueryable<AutomationItemSource> GetAutomationItems()
        {
            var l = new List<AutomationItemSource>();
            foreach (var j in FacturaService.Automators)
            {
                l.Add(new AutomationItemSource() { AutomationItemGuid = j.Guid });
            }
            return l.AsQueryable();
        }

        public static IQueryable<AutomationItemSource> AutomationItems { get; } = GetAutomationItems();

        public Guid AutomationItemGuid { get; set; }

        public override string ToString()
        {
            return FacturaService.Automators.FirstOrDefault(p => p.Guid == AutomationItemGuid)?.GetType().NameOf() ?? "n/a";
        }
    }
}