using System;
using System.Collections.Generic;
using System.Linq;
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

        public object GetContabTree()
        {
            var tree = new Dictionary<Cuenta, decimal>();
            var lst = new Dictionary<SubCuenta, decimal>();
            var ld = new Divisa() { Name = "Divisa local", Id = "es-HN" };
            foreach (var j in Partidas.SelectMany(p => p.Movimientos.GroupBy(q => q.Cuenta.Divisa ?? ld)))
            {
                foreach (var k in j)
                {
                    if (!lst.ContainsKey(k.Cuenta))
                    {
                        lst.Add(k.Cuenta, k.RawValue);
                    }
                    else
                    {
                        lst[k.Cuenta] += k.RawValue;
                    }
                }

                foreach (var k in lst)
                {
                    if (!tree.ContainsKey(k.Key.Parent))
                    {
                        tree.Add(k.Cuenta, k.RawValue);
                    }
                    else
                    {
                        tree[k.Key.Parent] += k.Value;
                    }
                }

            }


        }

        public struct PeriodoContabTreeItem
        {
            public string FullCode { get; internal set; }
            public string Displayname { get; internal set; }

        }
    }
}
