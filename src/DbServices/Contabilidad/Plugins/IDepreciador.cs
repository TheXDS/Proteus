using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using TheXDS.MCART.PluginSupport.Legacy;
using TheXDS.MCART.Types.Base;

namespace TheXDS.Proteus.Plugins
{
    public interface IDepreciador : IPlugin, IExposeGuid
    {
        decimal Calc(decimal initialValue, decimal residualValue, int totalCycles, int currentCycle);
    }

    [Guid("f111afca-04b4-4d83-8097-f87f5a309a6e")]
    public class LinearDepreciacion : Plugin, IDepreciador
    {
        public override string Name => "Depreciación lineal";
        public override string? Description => "Permite depreciar un inventario fijo por medio del método lineal. Cada periodo, el inventario se depreciará por un valor fijo hasta alcanzar el valor residual deseado.";
        public decimal Calc(decimal initialValue, decimal residualValue, int totalCycles, int currentCycle)
        {
            return (initialValue-residualValue) * (decimal)(1f / totalCycles);
        }
    }
}
