using System.Runtime.InteropServices;
using TheXDS.MCART.Attributes;
using TheXDS.Proteus.Api;
using TheXDS.Proteus.Models;
using TheXDS.Proteus.Models.Base;

namespace TheXDS.Proteus.Component
{
    [Name("Creación de Tarjetas de regalo")]
    [Guid("93b7f8f8-398f-4591-9f01-a8189b631157")]
    public class GiftCardCreationAutomator : IFacturableAutomation
    {
        public void OnFacturate(Factura f, Facturable item, int qty)
        {
            while (qty-- > 0)
            {
                Proteus.Service<FacturaService>()!.Add(new GiftCard { Amount = item.Precio });
            }
        }
    }
}