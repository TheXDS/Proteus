using System.Collections.Generic;
using TheXDS.Proteus.Models.Base;

namespace TheXDS.Proteus.Models
{
    public class Proveedor : Addressable<int>
    {
        public string Rtn { get; set; }
        public int DaysDue { get; set; } = 30;
        public virtual List<ProveedorXEmpresa> Empresas { get; set; } = new List<ProveedorXEmpresa>();
    }
}