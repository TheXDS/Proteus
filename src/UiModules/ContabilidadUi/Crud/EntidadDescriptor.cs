using System;
using System.Linq;
using TheXDS.Proteus.ContabilidadUi.Modules;
using TheXDS.Proteus.Crud.Base;
using TheXDS.Proteus.Models;
using TheXDS.Proteus.Models.Base;
using static TheXDS.Proteus.Annotations.InteractionType;

namespace TheXDS.Proteus.ContabilidadUi.Crud
{
    /// <summary>
    /// Describe las propiedades Crud para el modelo
    /// <see cref="Entidad"/>.
    /// </summary>
    public class EntidadDescriptor : CrudDescriptor<Entidad>
    {
        /// <summary>
        /// Describe las propiedades Crud para el modelo
        /// <see cref="Entidad"/>.
        /// </summary>
        protected override void DescribeModel()
        {
            OnModuleMenu(AdminTool);
            Property(p => p.Name).AsName();
            BeforeSave(SetParent);

            CanDelete(ChkNoPartidas);
        }

        private bool ChkNoPartidas(Entidad arg)
        {
            return !arg.Partidas.Any();
        }

        private void SetParent(Entidad arg1, ModelBase arg2)
        {
            arg1.Parent = arg2 as Empresa ?? ContabilidadModule.ModuleStatus.ActiveEmpresa ?? throw new Exception();
        }
    }

}