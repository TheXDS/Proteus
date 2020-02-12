using System;
using System.Collections.Generic;
using System.Linq;
using TheXDS.Proteus.Models.Base;

namespace TheXDS.Proteus.Models
{
    public class Empresa : Addressable<int>
    {
        public string? RTN { get; set; }
        public virtual List<Periodo> Periodos { get; set; } = new List<Periodo>();

        public virtual Cuenta Activo { get; set; }
        public virtual Cuenta Pasivo { get; set; }
        public virtual Cuenta Patrimonio { get; set; }
        public virtual Cuenta Ingresos { get; set; }
        public virtual Cuenta Costos { get; set; }
        public virtual Cuenta Gastos { get; set; }
        public virtual List<Entidad> Entidades { get; set; } = new List<Entidad>();
        public virtual List<CtaXPagar> CtasXPagar { get; set; } = new List<CtaXPagar>();
        
        public IEnumerable<SubCuenta> Flatten()
        {
            return Flatten(Activo)
                    .Concat(Flatten(Pasivo))
                    .Concat(Flatten(Patrimonio))
                    .Concat(Flatten(Ingresos))
                    .Concat(Flatten(Costos))
                    .Concat(Flatten(Gastos));
        }
        private static IEnumerable<SubCuenta> Flatten(Cuenta? c)
        {
            return c?.Children.SelectMany(Flatten).Concat(c?.SubCuentas) ?? Array.Empty<SubCuenta>();
        }
    }
}
