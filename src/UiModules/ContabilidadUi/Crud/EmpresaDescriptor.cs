using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TheXDS.MCART.Types.Base;
using TheXDS.MCART.ViewModel;
using TheXDS.Proteus.Api;
using TheXDS.Proteus.ContabilidadUi.Modules;
using TheXDS.Proteus.ContabilidadUi.ViewModels;
using TheXDS.Proteus.Crud.Base;
using TheXDS.Proteus.Models;
using TheXDS.Proteus.Models.Base;
using TheXDS.Proteus.ViewModels;
using TheXDS.Proteus.ViewModels.Base;
using static TheXDS.Proteus.Annotations.InteractionType;

namespace TheXDS.Proteus.ContabilidadUi.Crud
{
    /// <summary>
    /// Describe las propiedades Crud para el modelo
    /// <see cref="Empresa"/>.
    /// </summary>
    public class EmpresaDescriptor : CrudDescriptor<Empresa, EmpresaViewModel>
    {
        protected override void DescribeModel()
        {
            OnModuleMenu(AdminTool);

            Property(p => p.Name).AsName().AsListColumn();
            TextProperty(p => p.RTN).Mask("0000-0000-000000").Nullable().AsListColumn();
            this.DescribeAddress();
            this.DescribeContact();
            ListProperty(p => p.Periodos).Creatable().WatermarkAlwaysVisible().Label("Periodos contables");
            ShowAllInDetails();

            VmObjectProperty(p => p.FromMolde).Selectable()
                .Bind(UIElement.IsEnabledProperty, nameof(EmpresaViewModel.CanAddMolde))
                .Nullable()
                .Label("Crear árbol contable desde plantilla");

            VmBeforeSave(CreateRoot);
            CustomAction("Abrir nuevo período", NewPeriod);
            AfterSave(async ()=> await ContabilidadModule.ModuleStatus.InitViewModel());
        }

        private void CreateRoot(EmpresaViewModel arg1, ModelBase arg2)
        {
            if (arg1.FromMolde is { } m)
            {
                (arg1.Entity.Activo = m.Activo).Prefix = 1;
                (arg1.Entity.Pasivo = m.Pasivo).Prefix = 2;
                (arg1.Entity.Patrimonio = m.Patrimonio).Prefix = 3;
                (arg1.Entity.Ingresos = m.Ingresos).Prefix = 4;
                (arg1.Entity.Costos = m.Costos).Prefix = 5;
                (arg1.Entity.Gastos = m.Gastos).Prefix = 6;
            }

            else
            {
                arg1.Entity.Activo = new Cuenta() { Name = "Activo", Prefix = 1 };
                arg1.Entity.Pasivo = new Cuenta() { Name = "Pasivo", Prefix = 2 };
                arg1.Entity.Patrimonio = new Cuenta() { Name = "Patrimonio", Prefix = 3 };
                arg1.Entity.Ingresos = new Cuenta() { Name = "Ingresos", Prefix = 4 };
                arg1.Entity.Costos = new Cuenta() { Name = "Costos", Prefix = 5 };
                arg1.Entity.Gastos = new Cuenta() { Name = "Gastos", Prefix = 6 };
            }
        }

        private async void NewPeriod(Empresa obj, NotifyPropertyChangeBase vm)
        {
            App.Module<ContabilidadModule>()!.Reporter?.UpdateStatus("Abriendo nuevo periodo contable...");
            await Proteus.Service<ContabilidadService>()!.NewPeriod(obj);
            await ProteusViewModel.FullRefreshVmAsync<ContabManagerViewModel>();
            App.Module<ContabilidadModule>()!.Reporter?.Done();
        }
    }
}