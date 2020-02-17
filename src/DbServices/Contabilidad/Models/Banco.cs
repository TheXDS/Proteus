using System.Collections.Generic;
using TheXDS.Proteus.Models.Base;
using System.Linq;

namespace TheXDS.Proteus.Models
{
    public class Banco : Contact<int>
    {
        public virtual List<CuentaBanco> Cuentas { get; set; } = new List<CuentaBanco>();

        public IEnumerable<CuentaBanco> CuentasFor(Empresa e)
        {
            if (e is null) throw new System.ArgumentNullException(nameof(e));
            return Cuentas.Where(p => p.Cuenta.FindRoot().RootParent?.Id == e.Id);
        }
    }
}