using System;
using System.Collections.Generic;
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
    }
}
