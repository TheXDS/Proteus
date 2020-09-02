/*
Copyright © 2017-2020 César Andrés Morgan
Licenciado para uso interno solamente.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using TheXDS.MCART;
using TheXDS.MCART.Exceptions;
using TheXDS.MCART.Types;
using TheXDS.MCART.Types.Extensions;
using TheXDS.Proteus.Models.Base;
using System.Text;

namespace TheXDS.Proteus.Crud.Base
{
    public static partial class PropertyDescriptions
    {
        public static IPropertyDescriptor<TModel, TProperty> Range<TModel, TProperty>(this IPropertyDescriptor<TModel, TProperty> descriptor, TProperty min, TProperty max) where TModel : ModelBase where TProperty : IComparable<TProperty>
        {
            descriptor.SetValue(DescriptionValue.Range, new Range<TProperty>(min, max));
            return descriptor;
        }

        public static IPropertyDescriptor<TModel, TProperty?> Range<TModel, TProperty>(this IPropertyDescriptor<TModel, TProperty?> descriptor, TProperty min, TProperty max) where TModel : ModelBase where TProperty : struct, IComparable<TProperty>
        {
            descriptor.SetValue(DescriptionValue.Range, new Range<TProperty>(min, max));
            return descriptor;
        }

        public static IPropertyDescriptor<TModel, DateTime> WithTime<TModel>(this IPropertyDescriptor<TModel, DateTime> descriptor) where TModel : ModelBase
        {
            descriptor.SetValue(DescriptionValue.WithTime, true);
            return descriptor;
        }
        public static IPropertyDescriptor<TModel, DateTime?> WithTime<TModel>(this IPropertyDescriptor<TModel, DateTime?> descriptor) where TModel : ModelBase
        {
            descriptor.SetValue(DescriptionValue.WithTime, true);
            return descriptor;
        }
    }
}
