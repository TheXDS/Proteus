/*
Copyright © 2017-2020 César Andrés Morgan
Licenciado para uso interno solamente.
*/

using TheXDS.Proteus.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using TheXDS.MCART;
using TheXDS.MCART.Types.Extensions;
using TheXDS.Proteus.Component.Attributes;
using System.Collections;

namespace TheXDS.Proteus.Crud.Base
{
    /*
    internal class CrudPropertyDescriptor : IPropertyDescriptor, IPropertyDescription, IEquatable<CrudPropertyDescriptor>
    {
        private readonly Dictionary<DependencyProperty, BindingBase> _customBindings = new Dictionary<DependencyProperty, BindingBase>();



        public IDictionary<DependencyProperty, BindingBase> CustomBindings => _customBindings;
        Func<ModelBase, PropertyInfo, IEnumerable<ValidationError>> IPropertyDescription.Validator => (m, c) => _validators.SelectMany(p => p.Invoke(m, c));



        public IPropertyDescriptor Required()
        {
            return Nullability(NullMode.Required).Validator(CheckNotNull);
        }

        private IEnumerable<ValidationError> CheckNotNull(ModelBase arg1, PropertyInfo arg2)
        {
            if (arg1 is null) yield return new NullValidationError(arg2);
        }


        public bool Equals(CrudPropertyDescriptor other) => Property == other.Property;







        public IPropertyDescriptor Bind(DependencyProperty path, BindingBase binding)
        {
            CustomBindings.Add(path, binding);
            return this;
        }
        public IPropertyDescriptor Bind(DependencyProperty path, string binding)
        {
            return Bind(path, new Binding(binding));
        }
        public IPropertyDescriptor Bind(DependencyProperty path, string binding, object source)
        {
            return Bind(path, new Binding(binding) { Source = source });
        }
        public IPropertyDescriptor Bind(DependencyProperty path, PropertyPath binding)
        {
            return Bind(path, new Binding() { Path = binding });
        }
        public IPropertyDescriptor Bind(DependencyProperty path, PropertyPath binding, object source)
        {
            return Bind(path, new Binding() { Path = binding, Source = source });
        }

        #endregion
    }
    */

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

    /// <summary>
    /// Implementa la mayoría de descripciones comunes de propiedades como
    /// métodos de extensión genéricos.
    /// </summary>
    public static class PropertyDescriptions
    {
        /// <summary>
        /// Establece un valor predeterminado para el campo.
        /// </summary>
        /// <param name="value">Valor predeterminado a asignar.</param>
        /// <param name="descriptor">Descriptor a configurar.</param>
        /// <returns>
        /// Una referencia a la misma instancia para utilizar sintáxis
        /// Fluent.
        /// </returns>
        public static IPropertyDescriptor<TModel, TProperty> Default<TModel, TProperty>(this IPropertyDescriptor<TModel, TProperty> descriptor, TProperty value) where TModel : ModelBase
        {
            descriptor.SetValue(DescriptionValue.Default, value);
            return descriptor;
        }

        /// <summary>
        /// Indica que el campo no se mostrará en la interfaz del editor.
        /// </summary>
        /// <param name="descriptor">Descriptor a configurar.</param>
        /// <returns>
        /// Una referencia a la misma instancia para utilizar sintáxis
        /// Fluent.
        /// </returns>
        public static IPropertyDescriptor<TModel, TProperty> Hidden<TModel, TProperty>(this IPropertyDescriptor<TModel, TProperty> descriptor) where TModel : ModelBase
        {
            descriptor.SetValue(DescriptionValue.Visible, false);
            return descriptor;
        }

        /// <summary>
        /// Indica que una propiedad aparecerá en el Crud como elemento de
        /// solo lectura.
        /// </summary>
        /// <param name="descriptor">Descriptor a configurar.</param>
        /// <returns>
        /// Una referencia a la misma instancia para utilizar sintáxis
        /// Fluent.
        /// </returns>
        public static IPropertyDescriptor<TModel, TProperty> ReadOnly<TModel, TProperty>(this IPropertyDescriptor<TModel, TProperty> descriptor) where TModel : ModelBase
        {
            descriptor.SetValue(DescriptionValue.ReadOnly, true);
            return descriptor;
        }

        /// <summary>
        /// Indica que una propiedad aparecerá en el Crud como elemento de
        /// solo lectura.
        /// </summary>
        /// <param name="format">
        /// Formato de texto a aplicar al control.
        /// </param>
        /// <param name="descriptor">Descriptor a configurar.</param>
        /// <returns>
        /// Una referencia a la misma instancia para utilizar sintáxis
        /// Fluent.
        /// </returns>
        public static IPropertyDescriptor<TModel, TProperty> ReadOnly<TModel, TProperty>(this IPropertyDescriptor<TModel, TProperty> descriptor, string format) where TModel : ModelBase
        {
            descriptor.SetValue(DescriptionValue.Format, format);
            return ReadOnly(descriptor);
        }

        /// <summary>
        /// Establece un ícono a aplicar a los controles que lo soporten.
        /// </summary>
        /// <param name="icon">Ícono a aplicar.</param>
        /// <param name="descriptor">Descriptor a configurar.</param>
        /// <returns>
        /// Una referencia a la misma instancia para utilizar sintáxis
        /// Fluent.
        /// </returns>
        public static IPropertyDescriptor<TModel, TProperty> Icon<TModel, TProperty>(this IPropertyDescriptor<TModel, TProperty> descriptor, string icon) where TModel : ModelBase
        {
            descriptor.SetValue(DescriptionValue.Icon, icon);
            return descriptor;
        }

        /// <summary>
        /// Establece un ícono a aplicar a los controles que lo soporten.
        /// </summary>
        /// <param name="icon">Ícono a aplicar.</param>
        /// <param name="descriptor">Descriptor a configurar.</param>
        /// <returns>
        /// Una referencia a la misma instancia para utilizar sintáxis
        /// Fluent.
        /// </returns>
        public static IPropertyDescriptor<TModel, TProperty> Icon<TModel, TProperty>(this IPropertyDescriptor<TModel, TProperty> descriptor, char icon) where TModel : ModelBase
        {
            descriptor.SetValue(DescriptionValue.Icon, icon);
            return descriptor;
        }

        /// <summary>
        /// Establece una etiqueta a aplicar a los controles que lo
        /// soporten.
        /// </summary>
        /// <param name="label">Etiqueta a aplicar.</param>
        /// <param name="descriptor">Descriptor a configurar.</param>
        /// <returns>
        /// Una referencia a la misma instancia para utilizar sintáxis
        /// Fluent.
        /// </returns>
        public static IPropertyDescriptor<TModel, TProperty> Label<TModel, TProperty>(this IPropertyDescriptor<TModel, TProperty> descriptor, string label) where TModel : ModelBase
        {
            descriptor.SetValue(DescriptionValue.Label, label);
            return descriptor;
        }

        /// <summary>
        /// Indica que el campo es requerido, y por lo tanto no nulable.
        /// </summary>
        /// <param name="descriptor">Descriptor a configurar.</param>
        /// <returns>
        /// Una referencia a la misma instancia para utilizar sintáxis
        /// Fluent.
        /// </returns>
        public static IPropertyDescriptor<TModel, TProperty> Required<TModel, TProperty>(this IPropertyDescriptor<TModel, TProperty> descriptor) where TModel : ModelBase
        {
            descriptor.SetValue(DescriptionValue.Nullability, NullMode.Required);
            return descriptor;
        }

        /// <summary>
        /// Indica que el campo es nulable, por lo que su control será
        /// contenido por un CheckBox.
        /// </summary>
        /// <param name="descriptor">Descriptor a configurar.</param>
        /// <returns>
        /// Una referencia a la misma instancia para utilizar sintáxis
        /// Fluent.
        /// </returns>
        public static IPropertyDescriptor<TModel, TProperty> Nullable<TModel, TProperty>(this IPropertyDescriptor<TModel, TProperty> descriptor) where TModel : ModelBase
        {
            descriptor.SetValue(DescriptionValue.Nullability, NullMode.Nullable);
            return descriptor;
        }

        /// <summary>
        /// Indica que el campo es nulable, por lo que su control será
        /// contenido por un RadioButton.
        /// </summary>
        /// <param name="descriptor">Descriptor a configurar.</param>
        /// <returns>
        /// Una referencia a la misma instancia para utilizar sintáxis
        /// Fluent.
        /// </returns>
        public static IPropertyDescriptor<TModel, TProperty> RadioSelectable<TModel, TProperty>(this IPropertyDescriptor<TModel, TProperty> descriptor) where TModel : ModelBase
        {
            descriptor.SetValue(DescriptionValue.Nullability, NullMode.Radio);
            return descriptor;
        }

        /// <summary>
        /// Indica que el campo es nulable, por lo que su control será
        /// contenido por un RadioButton.
        /// </summary>
        /// <param name="descriptor">Descriptor a configurar.</param>
        /// <param name="groupId">
        /// Id del grupo de RadioButton al cual asociar al selector.
        /// </param>
        /// <returns>
        /// Una referencia a la misma instancia para utilizar sintáxis
        /// Fluent.
        /// </returns>
        public static IPropertyDescriptor<TModel, TProperty> RadioSelectable<TModel, TProperty>(this IPropertyDescriptor<TModel, TProperty> descriptor, string groupId) where TModel : ModelBase
        {
            descriptor.SetValue(DescriptionValue.Nullability, NullMode.Radio);
            descriptor.SetValue(DescriptionValue.RadioGroup, groupId);
            return descriptor;
        }

        /// <summary>
        /// Establece un valor ordinal al campo.
        /// </summary>
        /// <param name="descriptor">Descriptor a configurar.</param>
        /// <param name="order">Valor ordinal del campo.</param>
        /// <returns>
        /// Una referencia a la misma instancia para utilizar sintáxis
        /// Fluent.
        /// </returns>
        public static IPropertyDescriptor<TModel, TProperty> Order<TModel, TProperty>(this IPropertyDescriptor<TModel, TProperty> descriptor, int order) where TModel : ModelBase
        {
            descriptor.SetValue(DescriptionValue.Order, order);
            return descriptor;
        }

        /// <summary>
        /// Establece la función de validación de este campo.
        /// </summary>
        /// <typeparam name="TModel">Modelo del descriptor.</typeparam>
        /// <typeparam name="TProperty">
        /// Tipo de la propiedad descrita.
        /// </typeparam>
        /// <param name="descriptor">Descriptor a configurar.</param>
        /// <param name="validator">
        /// Función de validación a utilizar.
        /// </param>
        /// <returns>
        /// Una referencia a la misma instancia para utilizar sintáxis
        /// Fluent.
        /// </returns>
        public static IPropertyDescriptor<TModel, TProperty> Validator<TModel, TProperty>(this IPropertyDescriptor<TModel, TProperty> descriptor, Func<TModel, PropertyInfo, IEnumerable<ValidationError>> validator) where TModel : ModelBase
        {
            IEnumerable<ValidationError> CastTest(ModelBase m, PropertyInfo p)
            {
                if (!(m is TModel e)) return new ValidationError[] { $"Error general de validación para {typeof(TModel).NameOf()}. No se puede validar un {m?.GetType().NameOf() ?? "objeto de referencia nula (null)"}" };
                return validator(e, p);
            }
            if (descriptor[DescriptionValue.Validations] is null)
            {
                descriptor.SetValue(DescriptionValue.Validations, new List<Func<ModelBase, PropertyInfo, IEnumerable<ValidationError>>>());
            }
            ((List<Func<ModelBase, PropertyInfo, IEnumerable<ValidationError>>>)descriptor[DescriptionValue.Validations]!).Add(CastTest);
            return descriptor;
        }

        /// <summary>
        /// Establece un texto de ayuda sobre los controles generados a
        /// partir de este descriptor.
        /// </summary>
        /// <typeparam name="TModel">Modelo del descriptor.</typeparam>
        /// <typeparam name="TProperty">
        /// Tipo de la propiedad descrita.
        /// </typeparam>
        /// <param name="descriptor">Descriptor a configurar.</param>
        /// <param name="tooltip">Etiqueta a aplicar.</param>
        /// <returns>
        /// Una referencia a la misma instancia para utilizar sintáxis
        /// Fluent.
        /// </returns>
        public static IPropertyDescriptor<TModel, TProperty> Tooltip<TModel, TProperty>(this IPropertyDescriptor<TModel, TProperty> descriptor, string tooltip) where TModel : ModelBase
        {
            descriptor.SetValue(DescriptionValue.Tooltip, tooltip);
            return descriptor;
        }

        /// <summary>
        /// Indica que el control mostrará la marca de agua siempre.
        /// </summary>
        /// <typeparam name="TModel">Modelo del descriptor.</typeparam>
        /// <typeparam name="TProperty">
        /// Tipo de la propiedad descrita.
        /// </typeparam>
        /// <param name="descriptor">Descriptor a configurar.</param>
        /// <returns>
        /// Una referencia a la misma instancia para utilizar sintáxis
        /// Fluent.
        /// </returns>
        public static IPropertyDescriptor<TModel, TProperty> WatermarkAlwaysVisible<TModel, TProperty>(this IPropertyDescriptor<TModel, TProperty> descriptor) where TModel : ModelBase
        {
            descriptor.SetValue(DescriptionValue.WatermarkVisibility, true);
            return descriptor;
        }

        /// <summary>
        /// Establece un formato de presentación para una propiedad.
        /// </summary>
        /// <typeparam name="TModel">Modelo del descriptor.</typeparam>
        /// <typeparam name="TProperty">
        /// Tipo de la propiedad descrita.
        /// </typeparam>
        /// <param name="descriptor">Descriptor a configurar.</param>
        /// <param name="format">Formato a aplicar.</param>
        /// <returns>
        /// Una referencia a la misma instancia para utilizar sintáxis
        /// Fluent.
        /// </returns>
        public static IPropertyDescriptor<TModel, TProperty> Format<TModel, TProperty>(this IPropertyDescriptor<TModel, TProperty> descriptor, string format) where TModel : ModelBase
        {
            descriptor.SetValue(DescriptionValue.Format, format);
            return descriptor;
        }

        /// <summary>
        /// Agrega esta propiedad como columna de lista al presentarse en
        /// un control <see cref="System.Windows.Controls.ListView"/>.
        /// </summary>
        /// <typeparam name="TModel">Modelo del descriptor.</typeparam>
        /// <typeparam name="TProperty">
        /// Tipo de la propiedad descrita.
        /// </typeparam>
        /// <param name="descriptor">Descriptor a configurar.</param>
        /// <returns>
        /// Una referencia a la misma instancia para utilizar sintáxis
        /// Fluent.
        /// </returns>
        public static IPropertyDescriptor<TModel, TProperty> AsListColumn<TModel, TProperty>(this IPropertyDescriptor<TModel, TProperty> descriptor) where TModel : ModelBase
        {
            descriptor.SetValue(DescriptionValue.AsListColumn, true);
            return descriptor;
        }

        /// <summary>
        /// Indica que la propiedad se mostrará en el panel autogenerado de
        /// detalles.
        /// </summary>
        /// <typeparam name="TModel">Modelo del descriptor.</typeparam>
        /// <typeparam name="TProperty">
        /// Tipo de la propiedad descrita.
        /// </typeparam>
        /// <param name="descriptor">Descriptor a configurar.</param>
        /// <returns>
        /// Una referencia a la misma instancia para utilizar sintáxis
        /// Fluent.
        /// </returns>
        public static IPropertyDescriptor<TModel, TProperty> ShowInDetails<TModel, TProperty>(this IPropertyDescriptor<TModel, TProperty> descriptor) where TModel : ModelBase
        {
            descriptor.SetValue(DescriptionValue.ShowInDetails, true);
            return descriptor;
        }





        public static IPropertyDescriptor<TModel, string> Big<TModel>(this IPropertyDescriptor<TModel, string> descriptor) where TModel : ModelBase
        {
            descriptor.SetValue(DescriptionValue.TextKind, TextKind.Big);
            return descriptor;
        }

        public static IPropertyDescriptor<TModel, string> MinLength<TModel>(this IPropertyDescriptor<TModel, string> descriptor, int minLength) where TModel : ModelBase
        {
            descriptor.SetValue(DescriptionValue.TextKind, TextKind.Big);
            return descriptor;
        }
















        public static IPropertyDescriptor<TModel, TProperty> Selectable<TModel, TProperty>(this IPropertyDescriptor<TModel, TProperty> descriptor) where TModel : ModelBase where TProperty : IList
        {
            //descriptor.SetValue(DescriptionValue.ShowInDetails, true);
            //return descriptor;
        }

    }
}