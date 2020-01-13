using System;
using System.Collections.Generic;
using TheXDS.Proteus.Models.Base;

namespace TheXDS.Proteus.Models
{
    public class Periodo : TimestampModel<int>, IVoidable
    {
        public virtual Entidad Parent { get; set; }

        public DateTime? Void { get ; set; }

        public override string ToString()
        {
            return Timestamp.Year.ToString();
        }

        public virtual List<Partida> Partidas { get; set; } = new List<Partida>();
    }
}
