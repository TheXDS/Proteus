using TheXDS.MCART.Types;
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
            VmObjectProperty(p => p.SelectedEmpresa).Selectable().Important("Empresa");
            ObjectProperty(p => p.DebitoCuenta)
                .Selectable()
                .VmSource<ProveedorXEmpresaViewModel>(GetObservable)
                .Required()
                .Important("Auxiliar de gastos pagados por anticipado");
            ObjectProperty(p => p.CreditoCuenta)
                .Selectable()
                .VmSource<ProveedorXEmpresaViewModel>(GetObservable)
                .Required()
                .Important("Auxiliar de cuentas por pagar");
            ObjectProperty(p => p.GastoCuenta)
                .Selectable()
                .VmSource<ProveedorXEmpresaViewModel>(GetObservable)
                .Required()
                .Important("Cuenta de gasto");

        }

        private ObservableListWrap<ModelBase> GetObservable(ProveedorXEmpresaViewModel p, CrudViewModelBase v)
        {
            return p.GetCurrentSubCuentas(v as ISearchViewModel);
        }
    }

}