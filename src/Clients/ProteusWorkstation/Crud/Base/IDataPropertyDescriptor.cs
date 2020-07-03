/*
Copyright © 2017-2020 César Andrés Morgan
Licenciado para uso interno solamente.
*/

using TheXDS.Proteus.Models.Base;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using TheXDS.MCART.Types;
using TheXDS.MCART.ViewModel;
using TheXDS.Proteus.ViewModels.Base;

namespace TheXDS.Proteus.Crud.Base
{
    /// <summary>
    /// Expone métodos de descripción para todas las propiedades de enlace 
    /// a datos.
    /// </summary>
    public interface IDataPropertyDescriptor : IPropertyDescriptor
    {
        /// <summary>
        /// Indica que se debe utilizar un origen específico para la
        /// obtención de datos.
        /// </summary>
        /// <param name="source">
        /// Origen de datos a utilizar.
        /// </param>
        /// <returns>
        /// Una referencia a la misma instancia para utilizar sintáxis
        /// Fluent.
        /// </returns>
        IDataPropertyDescriptor Source(IQueryable<ModelBase>? source);

        IDataPropertyDescriptor VmSource<T>(Func<T, ObservableListWrap<ModelBase>> source) where T : ViewModelBase;

        IDataPropertyDescriptor VmSource<TParent>(Func<TParent, CrudViewModelBase, ObservableListWrap<ModelBase>> source) where TParent : ViewModelBase;

    }
}