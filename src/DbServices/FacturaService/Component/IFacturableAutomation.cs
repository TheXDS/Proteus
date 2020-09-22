using System.Threading.Tasks;
using TheXDS.MCART.Types.Base;
using TheXDS.Proteus.Models;
using TheXDS.Proteus.Models.Base;

namespace TheXDS.Proteus.Component
{
    /// <summary>
    /// Define una serie de miembros a implementar por un tipo que permita
    /// definir acciones personalizadas a efectuar sobre un ítem facturable al
    /// ser facturado.
    /// </summary>
    public interface IFacturableAutomation : IExposeGuid
    {
        void OnFacturate(Factura f, Facturable item, int qty);
        Task OnFacturateAsync(Factura f, Facturable item, int qty)
        {
            return Task.Run(() => OnFacturate(f, item, qty));
        }
    }
}
