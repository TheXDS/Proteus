using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TheXDS.Proteus.ContabilidadUi.Modules;
using TheXDS.Proteus.ContabilidadUi.ViewModels;
using TheXDS.Proteus.Crud;
using TheXDS.Proteus.Crud.Base;
using TheXDS.Proteus.Models;
using TheXDS.Proteus.Models.Base;
using static TheXDS.Proteus.Annotations.InteractionType;

namespace TheXDS.Proteus.ContabilidadUi.Crud
{
    /// <summary>
    /// Describe las propiedades Crud para el modelo
    /// <see cref="Partida"/>.
    /// </summary>
    public class PartidaDescriptor : CrudDescriptor<Partida, PartidaViewModel>
    {
        protected override void DescribeModel()
        {
            OnModuleMenu(Operation | Essential, ContabilidadModule.CanOpen);

            DateProperty(p => p.Timestamp).WithTime().Important("Fecha/hora de partida").Default(DateTime.Now);
            Property(p => p.Description).Important("Descripción").NotEmpty().Required();
            ListProperty(p => p.Movimientos).Creatable().ShowInDetails().Label("Movimientos").Validator<Partida>(ChkCuadrada);
            VmProperty(p => p.Cuadre).Label("Valor de cuadre").ReadOnly();
            ListProperty(p => p.Documentos).Creatable().ShowInDetails().Label("Documentos de referencia");
            BeforeSave(SetPeriod);            
            CanCreate(o => o is Periodo || ContabilidadModule.ModuleStatus.ActivePeriodo is { });
        }

        private void SetPeriod(Partida partida, ModelBase arg2)
        {
            partida.Parent = arg2 as Periodo ?? ContabilidadModule.ModuleStatus.ActivePeriodo ?? throw new Exception();
        }

        private IEnumerable<ValidationError> ChkCuadrada(Partida partida, PropertyInfo prop)
        {
            var m = partida.Movimientos;
            if (!m.Any()) yield return new NullValidationError(prop);
            if (m.Sum(p => p.RawValue) != 0m) yield return new ValidationError(prop,"Los movimientos no cuadran.");
        }
    }
}