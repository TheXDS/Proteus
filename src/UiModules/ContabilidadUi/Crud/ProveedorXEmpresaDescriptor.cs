﻿using System;
using TheXDS.MCART.Types;
using TheXDS.Proteus.Api;
using TheXDS.Proteus.ContabilidadUi.Modules;
using TheXDS.Proteus.Crud.Base;
using TheXDS.Proteus.Models;
using TheXDS.Proteus.Models.Base;
using TheXDS.Proteus.ViewModels;
using TheXDS.Proteus.ViewModels.Base;

namespace TheXDS.Proteus.ContabilidadUi.Crud
{
    /// <summary>
    /// Describe las propiedades Crud para el modelo
    /// <see cref="ProveedorXEmpresa"/>.
    /// </summary>
    public class ProveedorXEmpresaDescriptor : CrudDescriptor<ProveedorXEmpresa, ProveedorXEmpresaViewModel>
    {
        /// <summary>
        /// Describe las propiedades Crud para el modelo
        /// <see cref="ProveedorXEmpresa"/>.
        /// </summary>
        protected override void DescribeModel()
        {
            VmObjectProperty(p => p.SelectedEmpresa)
                .Selectable()
                .Required()
                .Important("Empresa")
                .Default(ContabilidadModule.ModuleStatus?.ActiveEmpresa!);
            ObjectProperty(p => p.DebitoCuenta)
                .Selectable()
                .VmSource<ProveedorXEmpresaViewModel>(GetObservable)
                .Required().NotNull()
                .Important("Auxiliar de gastos pagados por anticipado");
            ObjectProperty(p => p.CreditoCuenta)
                .Selectable()
                .VmSource<ProveedorXEmpresaViewModel>(GetObservable)
                .Required().NotNull()
                .Important("Auxiliar de cuentas por pagar");
            ObjectProperty(p => p.GastoCuenta)
                .Selectable()
                .VmSource<ProveedorXEmpresaViewModel>(GetObservable)
                .Required().NotNull()
                .Important("Cuenta de gasto");

            VmBeforeSave(CheckEmpSelected);

            // HACK: Al editar entidades con propiedades desde un ViewModel, el estado no se actualiza correctamente. Es necesario forzar el guardado.
            AfterSave(ForcefullySave);
        }

        private async void ForcefullySave(ProveedorXEmpresa arg1, ModelBase arg2)
        {
            if (!arg1.IsNew) await Proteus.Service<ContabilidadService>()!.ForcefullySaveAsync();
        }

        private void CheckEmpSelected(ProveedorXEmpresaViewModel arg1, ModelBase arg2)
        {
            if (arg1.SelectedEmpresa is null) throw new Exception("Debe seleccionar una empresa primero.");
        }

        private ObservableListWrap<ModelBase> GetObservable(ProveedorXEmpresaViewModel p, CrudViewModelBase v)
        {
            return p.GetCurrentSubCuentas(v as ISearchViewModel);
        }
    }
}