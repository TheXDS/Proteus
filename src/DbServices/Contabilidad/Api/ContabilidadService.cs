using System.Collections.Generic;
using System.Linq;
using TheXDS.Proteus.Context;
using TheXDS.Proteus.Models;

namespace TheXDS.Proteus.Api
{
    public class ContabilidadService : Service<ContabilidadContext>
    {
        private static ContabilidadService Instance => Proteus.Service<ContabilidadService>();
        public static IEnumerable<Empresa> AllEmpresas => Instance?.All<Empresa>().ToList();
    }
}
