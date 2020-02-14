/*
Copyright © 2017-2020 César Andrés Morgan
Licenciado para uso interno solamente.
*/

using System;
using System.Collections.Generic;
using TheXDS.Proteus.Models;
using TheXDS.Proteus.Plugins;
using TheXDS.Proteus.ViewModels.Base;
using TheXDS.Proteus.Widgets;

namespace TheXDS.Proteus.ContabilidadUi.Plugins
{
    public class CtaPagoCrudTools : CrudTool<CtaXPagar>
    {
        public CtaPagoCrudTools() : base(CrudToolVisibility.Selected)
        {
        }

        public override IEnumerable<Launcher> GetLaunchers(IEnumerable<Type> models, ICrudViewModel? vm)
        {
            throw new NotImplementedException();
        }
    }
}
