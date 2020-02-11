using System;
using TheXDS.Proteus.ContabilidadUi.Modules;
using TheXDS.Proteus.Crud.Base;
using TheXDS.Proteus.Models;

namespace TheXDS.Proteus.ContabilidadUi.Crud
{
    /// <summary>
    /// Describe las propiedades Crud para el modelo
    /// <see cref="Periodo"/>.
    /// </summary>
    public class PeriodoDescriptor : CrudDescriptor<Periodo>
    {
        protected override void DescribeModel()
        {
            DateProperty(p => p.Timestamp).Important("Fecha de inicio de periodo").Default(new DateTime(DateTime.Today.Year, 1, 1));
            DateProperty(p => p.Void).Nullable().Label("Fecha de cierre de periodo").AsListColumn().ShowInDetails();
            ListProperty(p => p.Partidas).Creatable().Label("Partidas del período").ShowInDetails().Required();
            //AfterSave(ContabilidadModule.ModuleStatus.InitViewModel);
            AfterSave(async () => await ContabilidadModule.ModuleStatus.InitViewModel());

        }
    }
}