/*
Copyright © 2017-2020 César Andrés Morgan
Licenciado para uso interno solamente.
*/

using System;
using System.Collections.Generic;
using System.Reflection;
using TheXDS.Proteus.Crud.Base;
using TheXDS.Proteus.Models;
using TheXDS.Proteus.Models.Base;
using TheXDS.Proteus.ViewModels;
using TheXDS.Proteus.ViewModels.Base;
using static TheXDS.MCART.Types.Extensions.StringExtensions;

namespace TheXDS.Proteus.Crud
{
    public class AvisoCrudDescriptor : CrudDescriptor<Aviso>
    {
        protected override void DescribeModel()
        {
            FriendlyName("Aviso");

            Property(p => p.Timestamp)
                .Label("Fecha de creación")
                .Timestamp()
                .AsListColumn()
                .ShowInDetails()
                .ReadOnly();

            Property(p => p.Header)
                .Label("Título")
                .MaxLength(150)
                .Validator(CheckTitle)
                .ShowInDetails()
                .AsListColumn()
                .Required();

            Property(p => p.Body)
                .Label("Contenido")
                .Big()
                .MaxLength(4096)
                .Validator(CheckBody)
                .ShowInDetails()
                .Required();

            BeforeSave(SetCreationTime);
            CustomAction("Vista previa", o => Proteus.MessageTarget?.Show(o.Header, o.Body));
            AfterSave(NotifyViewModel);
        }

        private void NotifyViewModel(Aviso arg1, ModelBase arg2)
        {
            ProteusViewModel.FullRefreshVmAsync<HomeViewModel>();
        }

        private IEnumerable<ValidationError> CheckBody(Aviso m, PropertyInfo p)
        {
            if (m.Body.IsEmpty()) yield return new ValidationError(p, "Un aviso debe contener texto.");
            if (m.Body.Length > 4096) yield return new ValidationError(p, "El aviso es demasiado largo. El límite es de 4 KB");
        }

        private IEnumerable<ValidationError> CheckTitle(Aviso m, PropertyInfo p)
        {
            if (m.Header.IsEmpty()) yield return new ValidationError(p, "Se necesita un título");
            if (m.Header.Length > 150) yield return new ValidationError(p, "El título del aviso es demasiado largo. El límite es de 150 caracteres.");
        }

        private void SetCreationTime(Aviso obj)
        {
            obj.Timestamp = DateTime.Now;
        }
    }
}