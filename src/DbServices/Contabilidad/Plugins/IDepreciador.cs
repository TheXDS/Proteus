using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using TheXDS.MCART.PluginSupport.Legacy;
using TheXDS.MCART.Types.Base;

namespace TheXDS.Proteus.Plugins
{
    public interface IDepreciador : IExposeGuid, INameable, IDescriptible
    {
        decimal Calc(decimal initialValue, decimal residualValue, int totalCycles, int currentCycle);
        decimal CalcAbsolute(decimal initialValue, decimal residualValue, int totalCycles, int currentCycle)
        {
            var dc = 0m;
            var j = 1;
            while (j >= currentCycle)
            { 
                dc += Calc(initialValue, residualValue, totalCycles, j);
                j++;
            }
            return dc;
        }
    }

    [Guid("f111afca-04b4-4d83-8097-f87f5a309a6e")]
    public class LinearDepreciacion : IDepreciador
    {
        public string Name => "Depreciación lineal";

        public string? Description => 
            "Permite depreciar un inventario fijo por medio del método " +
            "lineal. Cada periodo, el inventario se depreciará por un valor " +
            "fijo hasta alcanzar el valor residual deseado.";

        public decimal Calc(decimal initialValue, decimal residualValue, int totalCycles, int currentCycle)
        {
            return (initialValue - residualValue) * (decimal)(1f / totalCycles);
        }
        public decimal CalcAbsolute(decimal initialValue, decimal residualValue, int totalCycles, int currentCycle)
        {
            return (initialValue - residualValue) * (decimal)((float)currentCycle / totalCycles);
        }
    }
}
