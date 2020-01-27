using System.Collections.Generic;
using System.Globalization;
using TheXDS.Proteus.Models.Base;

namespace TheXDS.Proteus.Models
{
    public class Divisa : Nameable<string>
    {
        public virtual List<SubCuenta> Cuentas { get; set; } = new List<SubCuenta>();

        public CultureInfo Culture => CultureInfo.CreateSpecificCulture(Id);
        public RegionInfo Region => new RegionInfo(Id);
    }
}