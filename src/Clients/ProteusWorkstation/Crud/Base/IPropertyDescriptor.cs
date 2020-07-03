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

    /*

/// <summary>
/// Define una serie de miembros a implementar por una clase que
/// permita describir la construcción de ventanas CRUD de una propiedad
/// utilizando sintáxis Fluent.
/// </summary>
public interface IPropertyDescriptor
{
    /// <summary>
    /// Agrega un nuevo <see cref="BindingBase"/> personalizado a
    /// aplicar al control generado.
    /// </summary>
    /// <param name="path">
    /// Propiedad del control a enlazar.
    /// </param>
    /// <param name="binding">
    /// Enlace de datos a utilizar.
    /// </param>
    /// <returns>
    /// Una referencia a la misma instancia para utilizar sintáxis
    /// Fluent.
    /// </returns>
    IPropertyDescriptor Bind(DependencyProperty path, BindingBase binding);

    /// <summary>
    /// Agrega un nuevo <see cref="BindingBase"/> personalizado a
    /// aplicar al control generado.
    /// </summary>
    /// <param name="path">
    /// Propiedad del control a enlazar.
    /// </param>
    /// <param name="binding">
    /// Ruta de la propiedad a enlazar.
    /// </param>
    /// <returns>
    /// Una referencia a la misma instancia para utilizar sintáxis
    /// Fluent.
    /// </returns>
    IPropertyDescriptor Bind(DependencyProperty path, string binding);

    /// <summary>
    /// Agrega un nuevo <see cref="BindingBase"/> personalizado a
    /// aplicar al control generado.
    /// </summary>
    /// <param name="path">
    /// Propiedad del control a enlazar.
    /// </param>
    /// <param name="binding">
    /// Ruta de la propiedad a enlazar.
    /// </param>
    /// <param name="source">
    /// Origen de datos del enlace.
    /// </param>
    /// <returns>
    /// Una referencia a la misma instancia para utilizar sintáxis
    /// Fluent.
    /// </returns>
    IPropertyDescriptor Bind(DependencyProperty path, string binding, object source);

    /// <summary>
    /// Agrega un nuevo <see cref="BindingBase"/> personalizado a
    /// aplicar al control generado.
    /// </summary>
    /// <param name="path">
    /// Propiedad del control a enlazar.
    /// </param>
    /// <param name="binding">
    /// Ruta de la propiedad a enlazar.
    /// </param>
    /// <returns>
    /// Una referencia a la misma instancia para utilizar sintáxis
    /// Fluent.
    /// </returns>
    IPropertyDescriptor Bind(DependencyProperty path, PropertyPath binding);

    /// <summary>
    /// Agrega un nuevo <see cref="BindingBase"/> personalizado a
    /// aplicar al control generado.
    /// </summary>
    /// <param name="path">
    /// Propiedad del control a enlazar.
    /// </param>
    /// <param name="binding">
    /// Ruta de la propiedad a enlazar.
    /// </param>
    /// <param name="source">
    /// Origen de datos del enlace.
    /// </param>
    /// <returns>
    /// Una referencia a la misma instancia para utilizar sintáxis
    /// Fluent.
    /// </returns>
    IPropertyDescriptor Bind(DependencyProperty path, PropertyPath binding, object source);

}
*/
}