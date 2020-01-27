using System.Collections.Generic;
using System.Reflection;
using TheXDS.Proteus.ContabilidadUi.ViewModels;
using TheXDS.Proteus.Crud;
using TheXDS.Proteus.Crud.Base;
using TheXDS.Proteus.Models;

namespace TheXDS.Proteus.ContabilidadUi.Crud
{
    /// <summary>
    /// Describe las propiedades Crud para el modelo
    /// <see cref="Movimiento"/>.
    /// </summary>
    public class MovimientoDescriptor : CrudDescriptor<Movimiento, MovimientoViewModel>
    {
        protected override void DescribeModel()
        {
            ObjectProperty(p => p.Cuenta)
                .Selectable()
                .Important("Cuenta afectada")
                .Required()
                .AsListColumn();
            ObjectProperty(p => p.CostCenter).Selectable().Nullable().Label("Centro de costo afectado").AsListColumn().ShowInDetails();
            Property(p => p.RawValue).Important("Valor del movimiento").Validator<Movimiento>(CheckNotZero);
            VmProperty(p => p.Debe).Important("Valor del Debe").OnlyInDetails();
            VmProperty(p => p.Haber).Important("Valor del Haber").OnlyInDetails();
            VmProperty(p => p.RealValue).Label("Valor real").OnlyInDetails();
            VmProperty(p => p.LocalValue).Label("Valor en moneda local").OnlyInDetails();
            Property(p => p.ExchangeRate).Nullable().Label("Tasa de cambio").ShowInDetails();
        }

        private IEnumerable<ValidationError> CheckNotZero(Movimiento arg1, PropertyInfo arg2)
        {
            if (arg1.RawValue == 0m) yield return new ValidationError(arg2, "El movimiento debe tener un valor");
        }
    }
}