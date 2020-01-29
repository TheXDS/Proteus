using TheXDS.Proteus.Crud.Base;
using TheXDS.Proteus.Models;
using TheXDS.Proteus.Models.Base;

namespace TheXDS.Proteus.ContabilidadUi.Crud
{
    /// <summary>
    /// Describe las propiedades Crud para el modelo
    /// <see cref="SubCuenta"/>.
    /// </summary>
    public class SubCuentaDescriptor : CrudDescriptor<SubCuenta>
    {
        protected override void DescribeModel()
        {
            Property(p => p.FullCode).Label("Código de cuenta").AsListColumn().ShowInDetails().Hidden();
            Property(p => p.Name).AsName();
            Property(p => p.Movimientos).OnlyInDetails("Movimientos de la partida");
            Template();
            BeforeSave(SetPrefix);
        }

        private void SetPrefix(SubCuenta arg1, ModelBase arg2)
        {
            if (!(arg2 is Cuenta c)) return;
            arg1.Prefix = c.FreeSubCuentaPrefix;
        }
    }
}