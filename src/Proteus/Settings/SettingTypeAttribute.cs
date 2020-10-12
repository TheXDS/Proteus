/*
Copyright © 2017-2020 César Andrés Morgan
Licenciado para uso interno solamente.
*/

using System;
using TheXDS.Proteus.Models;

namespace TheXDS.Proteus.Component
{
    /// <summary>
    /// Permite establecer el tipo de valor almacenado por el
    /// <see cref="Setting"/>, lo cual permite a Proteus realizar validaciones
    /// adicionales y perzonalizar el control de edición asociado.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class SettingTypeAttribute : Attribute
    {
        public SettingTypeAttribute(Type type)
        {
            Type = type;
        }

        public Type Type { get; }
    }
}