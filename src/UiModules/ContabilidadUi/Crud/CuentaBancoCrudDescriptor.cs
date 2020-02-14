using TheXDS.Proteus.Crud.Base;
using TheXDS.Proteus.Models;

namespace TheXDS.Proteus.ContabilidadUi.Crud
{
    /// <summary>
    /// Describe las propiedades Crud para el modelo
    /// <see cref="CuentaBanco"/>.
    /// </summary>
    public class CuentaBancoCrudDescriptor : CrudDescriptor<CuentaBanco>
    {
        /// <summary>
        /// Describe las propiedades Crud para el modelo
        /// <see cref="CuentaBanco"/>.
        /// </summary>
        protected override void DescribeModel()
        {
            Property(p => p.Id).Id("Número");
            Property(p => p.Kind).Important("Tipo de cuenta");
            ObjectProperty(p => p.Cuenta).Selectable().Required().Important("Auxiliar contable de registro");
        }
    }

}