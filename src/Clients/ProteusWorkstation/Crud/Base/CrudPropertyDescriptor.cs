/*
Copyright © 2017-2020 César Andrés Morgan
Licenciado para uso interno solamente.
*/

using System.Collections.Generic;
using System.Reflection;
using TheXDS.MCART;
using TheXDS.Proteus.Component.Attributes;
using TheXDS.Proteus.Models.Base;
using static TheXDS.MCART.Objects;

namespace TheXDS.Proteus.Crud.Base
{
    internal class CrudPropertyDescriptor<TModel, TProperty> : IPropertyDescriptor<TModel, TProperty> where TModel : ModelBase
    {
        private readonly Dictionary<DescriptionValue, object?> _values = new Dictionary<DescriptionValue, object?>();

        public PropertyInfo Property { get; }

        public PropertyLocation Location { get; }

        public object? this[DescriptionValue property] => _values.TryGetValue(property, out var o) ? o : property.GetAttr<DescriptionDefaultAttribute>()?.Value ?? null;

        public CrudPropertyDescriptor(PropertyInfo property, PropertyLocation location)
        {
            Property = property;
            Location = location;
        }

        public void SetValue(DescriptionValue property, object? value)
        {
            if (!_values.ContainsKey(property))
            {
                _values.Add(property, value);
            }
            else
            {
                _values[property] = value;
            }
        }
    }
}