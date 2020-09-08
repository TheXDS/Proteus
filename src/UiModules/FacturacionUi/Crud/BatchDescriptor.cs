using System;
using System.Buffers;
using System.Collections.Generic;
using System.Reflection;
using TheXDS.Proteus.Crud;
using TheXDS.Proteus.Crud.Base;
using TheXDS.Proteus.Models;
using TheXDS.Proteus.Models.Base;
using Xceed.Wpf.Toolkit;

namespace TheXDS.Proteus.FacturacionUi.Crud
{
    public abstract class BatchDescriptor<T> : CrudDescriptor<T> where T : Batch, new()
    {
        protected override void DescribeModel()
        {
            ObjectProperty(p => p.Bodega)
                .Selectable()
                .Label("Ubicación")
                .Required();

            ObjectProperty(p => p.Item)
                .Selectable()
                .Label("Elemento de inventario")
                .AsListColumn()
                .Required();

            ObjectProperty(p => p.Lote)
                .Creatable()
                .Label("Lote")
                .Validator<T>(ChkLote)
                .Required();

            DescribeBatch();

            Property(p => p.Qty)
                .ShowInDetails()
                .AsListColumn()
                .Label("Cantidad en existencia")
                .Hidden();

            ShowAllInDetails();
            AllListColumn();
        }

        private IEnumerable<ValidationError> ChkLote(T batch, PropertyInfo prop)
        {
            if ((batch.Item?.ExpiryDays.HasValue ?? false) && batch.Lote is null)
            {
                yield return new NullValidationError(prop, "El ítem ha sido marcado como expirable, por lo que es obligatorio especificar la información de Lote.");
            }
        }

        protected abstract void DescribeBatch();
    }
}