using TheXDS.Proteus.Context;
using TheXDS.Proteus.Models;

namespace TheXDS.Proteus.Api
{
    public class ContabilidadService : Service<ContabilidadContext>
    {
        public AccessControlList? GetList()
        {
            return GetUser<AccessControlList>(this);
        }
    }
}