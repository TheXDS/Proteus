using System;
using TheXDS.MCART.Types;
using TheXDS.Proteus.Api;
using TheXDS.Proteus.ContabilidadUi.Modules;
using TheXDS.Proteus.ContabilidadUi.ViewModels.Base;
using TheXDS.Proteus.Crud.Base;
using TheXDS.Proteus.Models;
using TheXDS.Proteus.Models.Base;
using TheXDS.Proteus.ViewModels;
using TheXDS.Proteus.ViewModels.Base;

namespace TheXDS.Proteus.ContabilidadUi.Crud
{
    /// <summary>
    /// Describe las propiedades Crud para el modelo
    /// <see cref="ClienteXEmpresa"/>.
    /// </summary>
    public abstract class XXEmpresaCrudDescriptor<TModel> : CrudDescriptor<TModel, XXEmpresaViewModel<TModel>> where TModel : ModelBase, IXXEmpresa, new()
    {
        /// <summary>
        /// Describe las propiedades Crud para el modelo
        /// <see cref="ClienteXEmpresa"/>.
        /// </summary>
        protected override void DescribeModel()
        {
            VmObjectProperty(p => p.SelectedEmpresa)
                .Selectable()
                .Required()
                .Important("Empresa")
                .Default(ContabilidadModule.ModuleStatus?.ActiveEmpresa!);
            DescribeSubModel();
            VmBeforeSave(CheckEmpSelected);

            // HACK: Al editar entidades con propiedades desde un ViewModel, el estado no se actualiza correctamente. Es necesario forzar el guardado.
            AfterSave(ForcefullySave);
        }

        protected abstract void DescribeSubModel();

        private async void ForcefullySave(TModel arg1, ModelBase arg2)
        {
            if (!arg1.IsNew) await Proteus.Service<ContabilidadService>()!.ForcefullySaveAsync();
        }

        private void CheckEmpSelected(XXEmpresaViewModel<TModel> arg1, ModelBase arg2)
        {
            if (arg1.SelectedEmpresa is null) throw new Exception("Debe seleccionar una empresa primero.");
        }

        protected ObservableListWrap<ModelBase> GetObservable(XXEmpresaViewModel<TModel> p, CrudViewModelBase v)
        {
            return p.GetCurrentSubCuentas(v as ISearchViewModel);
        }
    }


    /// <summary>
    /// Describe las propiedades Crud para el modelo
    /// <see cref="ClienteXEmpresa"/>.
    /// </summary>
    public class ClienteXEmpresaCrudDescriptor : XXEmpresaCrudDescriptor<ClienteXEmpresa>
    {
        /// <summary>
        /// Describe las propiedades Crud para el modelo
        /// <see cref="ClienteXEmpresa"/>.
        /// </summary>
        protected override void DescribeSubModel()
        {
            ObjectProperty(p => p.DebitoCuenta)
                .Selectable()
                .VmSource<ClienteXEmpresaViewModel>(GetObservable)
                .Required().NotNull()
                .Important("Auxiliar de cuentas por cobrar");
            ObjectProperty(p => p.CreditoCuenta)
                .Selectable()
                .VmSource<ClienteXEmpresaViewModel>(GetObservable)
                .Required().NotNull()
                .Important("Auxiliar de cuentas cobradas por anticipado");
            ObjectProperty(p => p.IngresoCuenta)
                .Selectable()
                .VmSource<ClienteXEmpresaViewModel>(GetObservable)
                .Required().NotNull()
                .Important("Cuenta de ingresos");
        }
    }
}