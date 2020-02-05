using System;
using System.Collections.Generic;
using System.Linq;
using TheXDS.MCART.Types;
using TheXDS.Proteus.Models.Base;

namespace TheXDS.Proteus.Models
{
    public class Periodo : TimestampModel<int>, IVoidable
    {
        public virtual Empresa Parent { get; set; }

        public DateTime? Void { get ; set; }

        public override string ToString()
        {
            return Timestamp.ToString("MMMM yyyy");
        }

        public virtual List<Partida> Partidas { get; set; } = new List<Partida>();

        public IEnumerable<IGrouping<Divisa, PeriodoContabTreeItem>> GetContabTree()
        {
            var ld = new Divisa() { Name = "Divisa local", Id = "es-HN" };

            foreach (var j in Partidas.SelectMany(p => p.Movimientos).GroupBy(q => q.Cuenta.Divisa ?? ld))
            {
                var lst = new List<PeriodoContabTreeItem>();
                foreach (var k in j)
                {
                    if (!Find(lst, k.Cuenta.FullCode, out var item))
                    {
                        lst.Add(PeriodoContabTreeItem.ToPcti(k.Cuenta, k.RawValue));
                    }
                    else
                    {
                        item.Value += k.RawValue;
                    }
                    foreach (var l in Enumerate2Root(k.Cuenta.Parent))
                    {
                        if (!Find(lst, l.FullCode, out item))
                        {
                            lst.Add(PeriodoContabTreeItem.ToPcti(l, k.RawValue));
                        }
                        else
                        {
                            item.Value += k.RawValue;
                        }
                    }
                }
                foreach (var k in lst)
                {
                    if (k.ParentCode != null && Find(lst, k.ParentCode, out var parent))
                    {
                        parent.Children.Add(k);
                    }
                }
                yield return new Grouping<Divisa, PeriodoContabTreeItem>(j.Key, lst.Where(p=>p.ParentCode == null));
            }
        }

        private static IEnumerable<Cuenta> Enumerate2Root(Cuenta c)
        {
            while (c != null)
            {
                yield return c;
                c = c.Parent!;
            }
        }

        private bool Find(List<PeriodoContabTreeItem> lst, string id, out PeriodoContabTreeItem item)
        {
            foreach (var j in lst)
            {
                if (j.FullCode == id)
                {
                    item = j;
                    return true;
                }
            }
            item = default;
            return false;
        }
        
        public class PeriodoContabTreeItem
        {
            public string FullCode { get; internal set; }
            public string? ParentCode { get; internal set; }
            public string DisplayName { get; internal set; }
            public bool Bold { get; internal set; }
            public List<PeriodoContabTreeItem> Children { get; internal set; }
            public decimal Value { get; internal set; }

            public static PeriodoContabTreeItem ToPcti(SubCuenta c, decimal value)
            {
                return new PeriodoContabTreeItem
                {
                    FullCode = c.FullCode,
                    ParentCode = c.Parent.FullCode,
                    DisplayName = c.Name,
                    Bold = false,
                    Children = new List<PeriodoContabTreeItem>(),
                    Value = value
                };
            }
            public static PeriodoContabTreeItem ToPcti(Cuenta c, decimal value)
            {
                return new PeriodoContabTreeItem
                {
                    FullCode = c.FullCode,
                    ParentCode = c.Parent?.FullCode,
                    DisplayName = c.Name,
                    Bold = true,
                    Children = new List<PeriodoContabTreeItem>(),
                    Value = value
                };
            }

        }
    }
}
