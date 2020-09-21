using TheXDS.Proteus.Crud.Base;
using TheXDS.Proteus.Models;
using TheXDS.Proteus.ViewModels;

namespace TheXDS.Proteus.ContabilidadUi.Crud
{
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