/*
Copyright © 2017-2020 César Andrés Morgan
Licenciado para uso interno solamente.
*/

#nullable enable

using System;
using TheXDS.MCART.Attributes;

namespace TheXDS.Proteus.Component.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class DefaultAttribute : TextAttribute
    {
        public DefaultAttribute(string? value) : base(value)
        {
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public sealed class DescriptionDefaultAttribute : Attribute, IValueAttribute<object?>
    {
        public DescriptionDefaultAttribute(object? value)
        {
            Value = value;
        }

        public object? Value { get; }
    }

}