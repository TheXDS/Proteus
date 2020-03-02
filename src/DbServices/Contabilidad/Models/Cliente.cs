using System.Collections.Generic;
using TheXDS.Proteus.Models.Base;

namespace TheXDS.Proteus.Models
{
    public class Cliente : Addressable<int>
    {
        public string? Rtn { get; set; }
        public virtual List<ClienteXEmpresa> Empresas { get; set; } = new List<ClienteXEmpresa>();

    }
}