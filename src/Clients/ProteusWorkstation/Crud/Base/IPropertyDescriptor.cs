/*
Copyright © 2017-2020 César Andrés Morgan
Licenciado para uso interno solamente.
*/

using System;
using TheXDS.Proteus.Models.Base;

namespace TheXDS.Proteus.Crud.Base
{
    /// <summary>
    /// Describe una serie de miembros a implementar por una clase que permita
    /// describir una propiedad de un modelo.
    /// </summary>
    public interface IPropertyDescriptor : IPropertyDescription
    {
        /// <summary>
        /// Establece un valor de descripción.
        /// </summary>
        /// <param name="property">
        /// Propiedad de descripción a establecer.
        /// </param>
        /// <param name="value">Valor de la propiedad de descripción.</param>
        void SetValue(DescriptionValue property, object? value);
    }

    /// <summary>
    /// Describe una serie de miembros a implementar por una clase que permita
    /// describir una propiedad de un modelo, incluyendo información
    /// fuertemente tipeada del modelo y de la propiedad descritos.
    /// </summary>
    public interface IPropertyDescriptor<out TModel, TProperty> : IPropertyDescriptor, IPropertyDescription where TModel : ModelBase
    {
        Type IPropertyDescription.ModelType => typeof(TModel);

        Type IPropertyDescription.PropertyType => typeof(TProperty);
    }
}