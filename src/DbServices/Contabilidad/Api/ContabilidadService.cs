using System.Threading.Tasks;
using TheXDS.Proteus.Component;
using TheXDS.Proteus.Context;
using TheXDS.Proteus.ViewModels;

namespace TheXDS.Proteus.Api
{
    public class ContabilidadService : Service<ContabilidadContext>
    {
        public static ContabManagerViewModel ModuleStatus { get; private set; }
        
        protected override Task AfterInitialization(IStatusReporter reporter)
        {            
            ModuleStatus = new ContabManagerViewModel();
            return ModuleStatus.InitViewModel();
        }
    }
}