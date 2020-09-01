/*
Copyright © 2017-2020 César Andrés Morgan
Licenciado para uso interno solamente.
*/

using System;
using System.Reflection;

namespace TheXDS.Proteus.Crud.Base
{
    /// <summary>
    /// Define una serie de miembros a implementar por un tipo que exponga
    /// una colección de valores de configuración de manera básica sin
    /// especialización de tipos de modelo ni de propiedad.
    /// </summary>
    public interface IPropertyDescription
    {
        /// <summary>
        /// Referencia a la propiedad descrita por este
        /// <see cref="IPropertyDescription"/>.
        /// </summary>
        PropertyInfo Property { get; }

        /// <summary>
        /// Obtiene un valor que indica el orígen de la propiedad descrita.
        /// </summary>
        PropertyLocation Location { get; }

        /// <summary>
        /// Obtiene un valor configurado para este
        /// <see cref="IPropertyDescription"/>.
        /// </summary>
        /// <param name="property">
        /// Valor configurado a buscar.
        /// </param>
        /// <returns>
        /// El valor configurado, o en caso de no haber sido especificado, se
        /// obtendrá un valor predeterminado predefinido para el valor, o
        /// <see langword="null"/> si no se ha predefinido el mismo.
        /// </returns>
        object? this[DescriptionValue property] { get; }

        /// <summary>
        /// Obtiene una referencia al tipo del modelo descrito.
        /// </summary>
        Type ModelType { get; }

        /// <summary>
        /// Obtiene una referencia al tipo de la propiedad descrita.
        /// </summary>
        Type PropertyType { get; }

        /// <summary>
        /// Obtiene una instancia de la clase <see cref="Column"/> construida
        /// con la información contenida en este
        /// <see cref="IPropertyDescription"/>.
        /// </summary>
        /// <returns>
        /// Una nueva instancia de la clase <see cref="Column"/> construida con
        /// la información contenida en este
        /// <see cref="IPropertyDescription"/>.
        /// </returns>
        Column AsColumn()
        {
            return new Column(
                (string?)this[DescriptionValue.Label] ?? Property.Name,
                Property.Name)
            {
                Format = (string?)this[DescriptionValue.Format]
            };
        }

        string GetBindingString()
        {
            return $"{(Location == PropertyLocation.Model ? "Entity." : null)}{Property.Name}";
        }
    }
}