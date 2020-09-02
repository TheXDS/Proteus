﻿/*
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

namespace TheXDS.Proteus.Crud.Base
{
    /// <summary>
    /// Implementa la mayoría de descripciones comunes de propiedades como
    /// métodos de extensión genéricos.
    /// </summary>
    public static partial class PropertyDescriptions
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
            descriptor.SetValue(DescriptionValue.UseDefault, true);
            return descriptor;
        }

        /// <summary>
        /// Obtiene un valor que indica si se utilizará el valor predeterminado
        /// configurado para la propiedad.
        /// </summary>
        /// <param name="description">
        /// Descriptor desde el cual obtener el valor configurado.
        /// </param>
        /// <returns>
        /// <see langword="true"/> se se debe utilizar un valor predeterminado
        /// específico para la propiedad, <see langword="false"/> en caso
        /// contrario.
        /// </returns>
        public static bool UseDefault(this IPropertyDescription description)
        {
            return (bool?)description[DescriptionValue.UseDefault] ?? false;
        }

        /// <summary>
        /// Obtiene el valor predeterminado establecido para este campo.
        /// </summary>
        /// <param name="description">
        /// Descriptor desde el cual obtener el valor configurado.
        /// </param>
        /// <returns>El valor configurado.</returns>
        public static object? Default(this IPropertyDescription description)
        {
            return description[DescriptionValue.Default];
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
        /// Obtiene un valor que indica si no se mostrará el campo en la
        /// interfaz del editor.
        /// </summary>
        /// <param name="description">
        /// Descriptor desde el cual obtener el valor configurado.
        /// </param>
        /// <returns>
        /// <see langword="true"/> si se debe ocultar el campo en el editor,
        /// <see langword="false"/> en caso contrario.
        /// </returns>
        public static bool Hidden(this IPropertyDescription description)
        {
            return !(bool?)description[DescriptionValue.Visible] ?? false;
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
        /// Obtiene un valor que indica si el control a generar debe ser de
        /// sólo lectura.
        /// </summary>
        /// <param name="description">
        /// Descriptor desde el cual obtener el valor configurado.
        /// </param>
        /// <returns>
        /// <see langword="true"/> si se debe generar un control de sólo
        /// lectura, <see langword="false"/> en caso contrario.
        /// </returns>
        public static bool ReadOnly(this IPropertyDescription description)
        {
            return (bool?)description[DescriptionValue.ReadOnly] ?? false;
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
        /// Obtiene una cadena con el ícono a utilizar al generar el control de
        /// CRUD.
        /// </summary>
        /// <param name="description">
        /// Descriptor desde el cual obtener el valor configurado.
        /// </param>
        /// <returns>El ícono a utilizar al generar el control de Crud.</returns>
        public static string Icon(this IPropertyDescription description)
        {
            return description[DescriptionValue.Icon] switch
            {
                string s => s,
                char c => new string(c, 1),
                _ => "✏"
            };
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
        /// Obtiene una cadena con el texto de etiqueta a utilizar al generar
        /// el control de CRUD.
        /// </summary>
        /// <param name="description">
        /// Descriptor desde el cual obtener el valor configurado.
        /// </param>
        /// <returns>
        /// El texto de etiqueta a utilizar al generar el control de Crud.
        /// </returns>
        public static string Label(this IPropertyDescription description)
        {
            return description[DescriptionValue.Label] as string ?? description.Property.Name;
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
        /// Obtiene una cadena con el texto de tooltip a utilizar al generar
        /// el control de CRUD.
        /// </summary>
        /// <param name="description">
        /// Descriptor desde el cual obtener el valor configurado.
        /// </param>
        /// <returns>
        /// El texto de tooltip a utilizar al generar el control de Crud.
        /// </returns>
        public static string Tooltip(this IPropertyDescription description)
        {
            return description[DescriptionValue.Tooltip] as string ?? description.Property.Name;
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
        /// Obtiene un valor que indica el orden en el cual el control generado
        /// debe colocarse en el CRUD.
        /// </summary>
        /// <param name="description">
        /// Descriptor desde el cual obtener el valor configurado.
        /// </param>
        /// <returns>
        /// Un valor que indica el orden en el cual el control generado debe
        /// colocarse en el CRUD, o <see langword="null"/> si el valor no ha
        /// sido configurado, caso en el cual el control se colocará de acuerdo
        /// al orden de descripción.
        /// </returns>
        public static int? Order(this IPropertyDescription description)
        {
            return description[DescriptionValue.Order] is int i ? (int?)i : null;
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
        /// Obtiene una cadena con el texto de tooltip a utilizar al generar
        /// el control de CRUD.
        /// </summary>
        /// <param name="description">
        /// Descriptor desde el cual obtener el valor configurado.
        /// </param>
        /// <returns>
        /// El texto de tooltip a utilizar al generar el control de Crud.
        /// </returns>
        public static string? Format(this IPropertyDescription description)
        {
            return description[DescriptionValue.Format] as string;
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
        /// Obtiene un valor que indica si la etiqueta de marca de agua debe
        /// ser siempre visible.
        /// </summary>
        /// <param name="description">
        /// Descriptor desde el cual obtener el valor configurado.
        /// </param>
        /// <returns>
        /// <see langword="true"/> si la etiqueta debe ser siempre visible,
        /// <see langword="false"/> en caso contrario.
        /// </returns>
        public static bool WatermarkAlwaysVisible(this IPropertyDescription description)
        {
            return description[DescriptionValue.WatermarkVisibility] is bool b && b;
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

        /// <summary>
        /// Obtiene un valor que indica si no se mostrará en el panel
        /// autogenerado de detalles.
        /// </summary>
        /// <param name="description">
        /// Descriptor desde el cual obtener el valor configurado.
        /// </param>
        /// <returns>
        /// <see langword="true"/> si se debe ocultar el campo en el panel
        /// autogenerado de detalles, <see langword="false"/> en caso
        /// contrario.
        /// </returns>
        public static bool ShowInDetails(this IPropertyDescription description)
        {
            return (bool?)description[DescriptionValue.ShowInDetails] ?? false;
        }

        public static IPropertyDescriptor<TModel, TProperty> Bind<TModel, TProperty>(this IPropertyDescriptor<TModel, TProperty> descriptor, DependencyProperty path, BindingBase binding) where TModel : ModelBase
        {
            if (!(descriptor[DescriptionValue.Bindings] is Dictionary<DependencyProperty, BindingBase> dic))
            {
                descriptor.SetValue(DescriptionValue.Bindings, dic = new Dictionary<DependencyProperty, BindingBase>());
            }
            dic.Add(path, binding);
            return descriptor;
        }
        
        public static IPropertyDescriptor<TModel, TProperty> Bind<TModel, TProperty>(this IPropertyDescriptor<TModel, TProperty> descriptor, DependencyProperty path, string binding) where TModel : ModelBase
        {
            return Bind(descriptor, path, new Binding(binding));
        }
        
        public static IPropertyDescriptor<TModel, TProperty> Bind<TModel, TProperty>(this IPropertyDescriptor<TModel, TProperty> descriptor, DependencyProperty path, string binding, object source) where TModel : ModelBase
        {
            return Bind(descriptor, path, new Binding(binding) { Source = source });
        }
        
        public static IPropertyDescriptor<TModel, TProperty> Bind<TModel, TProperty>(this IPropertyDescriptor<TModel, TProperty> descriptor, DependencyProperty path, PropertyPath binding) where TModel : ModelBase
        {
            return Bind(descriptor, path, new Binding() { Path = binding });
        }
        
        public static IPropertyDescriptor<TModel, TProperty> Bind<TModel, TProperty>(this IPropertyDescriptor<TModel, TProperty> descriptor, DependencyProperty path, PropertyPath binding, object source) where TModel : ModelBase
        {
            return Bind(descriptor, path, new Binding() { Path = binding, Source = source });
        }
        
        public static IDictionary<DependencyProperty, BindingBase> CustomBindings(this IPropertyDescription description)
        {
            return ((Dictionary<DependencyProperty, BindingBase>?)description[DescriptionValue.Bindings]) ?? new Dictionary<DependencyProperty, BindingBase>();
        }

        public static IEnumerable<Column> Columns(this IPropertyDescription description)
        {
            return ((IEnumerable<Column>?)description[DescriptionValue.ListColumns]) ?? Array.Empty<Column>();
        }

        public static IPropertyDescriptor<TModel, TProperty> Column<TModel, TProperty, TElement>(
            this IPropertyDescriptor<TModel, TProperty> descriptor, string header, Expression<Func<TElement, object>> property)
            where TModel : ModelBase
            where TProperty : IEnumerable<TElement>
            where TElement : ModelBase
        {
            if (!(descriptor[DescriptionValue.ListColumns] is List<Column> lst))
            {
                descriptor.SetValue(DescriptionValue.Bindings, lst = new List<Column>());
            }
            lst.Add(new Column(header, property.Name));
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

        public static IEnumerable<Func<ModelBase, PropertyInfo, IEnumerable<ValidationError>>> Validations(this IPropertyDescription description)
        {
            return ((List<Func<ModelBase, PropertyInfo, IEnumerable<ValidationError>>>?)description[DescriptionValue.Validations])
                ?? Array.Empty<Func<ModelBase, PropertyInfo, IEnumerable<ValidationError>>>().ToList();
        }

        public static Func<ModelBase, PropertyInfo, IEnumerable<ValidationError>> Validator(this IPropertyDescription description)
        {
            return (model, prop) => Validations(description).SelectMany(p => p.Invoke(model, prop));
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
            descriptor.Validator(CheckNotNull);
            return descriptor;
        }

        public static bool Required(this IPropertyDescription description)
        {
            return (description[DescriptionValue.Nullability] is NullMode n && n == NullMode.Required) || !InferNullability(description);
        }
        
        private static bool InferNullability(IPropertyDescription description)
        {
            return description.PropertyType.IsClass || System.Nullable.GetUnderlyingType(description.PropertyType) is { };
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

        public static bool Nullable(this IPropertyDescription description)
        {
            return (description[DescriptionValue.Nullability] is NullMode n && n == NullMode.Nullable) || InferNullability(description);
        }

        public static NullMode NullabilityMode(this IPropertyDescription description)
        {
            return description[DescriptionValue.Nullability] is NullMode n ? n : default;
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







        

        public static IPropertyDescriptor<TModel, TProperty> Selectable<TModel, TProperty>(this IPropertyDescriptor<TModel, TProperty> descriptor) where TModel : ModelBase where TProperty : ModelBase
        {
            descriptor.SetValue(DescriptionValue.Selectable, typeof(TProperty));
            return descriptor;
        }
        
        /// <summary>
        /// Indica que el valor asociado a este descriptor debe ser 
        /// editable.
        /// </summary>
        /// <returns>
        /// Una referencia a la misma instancia para utilizar sintáxis
        /// Fluent.
        /// </returns>
        public static IPropertyDescriptor<TModel, TProperty> Editable<TModel, TProperty>(this IPropertyDescriptor<TModel, TProperty> descriptor) where TModel : ModelBase where TProperty : ModelBase, new()
        {
            descriptor.SetValue(DescriptionValue.Editable, typeof(TProperty));
            return descriptor;
        }
        
        public static IPropertyDescriptor<TModel, TProperty> Creatable<TModel, TProperty>(this IPropertyDescriptor<TModel, TProperty> descriptor)
            where TModel : ModelBase
            where TProperty : ModelBase, new()
        {
            descriptor.SetValue(DescriptionValue.Creatable, typeof(TProperty));
            return descriptor;
        }
        
        public static IPropertyDescriptor<TModel, TProperty> Creatable<TModel, TProperty>(
            this IPropertyDescriptor<TModel, TProperty> descriptor,
            IEnumerable<Type> models)
            where TModel : ModelBase
            where TProperty : ModelBase
        {
            if (models.Any(p => !p.IsInstantiable())) throw new ClassNotInstantiableException();
            descriptor.SetValue(DescriptionValue.Creatable, models);
            return descriptor;
        }
   


        public static IPropertyDescriptor<TModel, TProperty> DisplayMember<TModel, TProperty>(
            this IPropertyDescriptor<TModel, TProperty> descriptor,
            Expression<Func<TProperty, object?>> selector)
            where TModel : ModelBase 
            where TProperty : ModelBase
        {
            descriptor.SetValue(DescriptionValue.DisplayMember, ReflectionHelpers.GetProperty(selector).Name);
            return descriptor;
        }
  
        public static IPropertyDescriptor<TModel, TProperty> DisplayMember<TModel, TProperty>(
            this IPropertyDescriptor<TModel, TProperty> descriptor,
            string bindingPath)
            where TModel : ModelBase
            where TProperty : ModelBase
        {
            descriptor.SetValue(DescriptionValue.DisplayMember, bindingPath);
            return descriptor;
        }

        public static string? DisplayMemberPath(this IPropertyDescription description)
        {
            return description[DescriptionValue.DisplayMember] as string;
        }

        public static IPropertyDescriptor<TModel, TProperty> AllowSelection<TModel, TProperty>(this IPropertyDescriptor<TModel, TProperty> descriptor) 
            where TModel : ModelBase
            where TProperty : IEnumerable<ModelBase>
        {
            descriptor.SetValue(DescriptionValue.Selectable, typeof(TProperty).ResolveCollectionType());
            return descriptor;
        }
  
        public static IPropertyDescriptor<TModel, TProperty> AllowEdit<TModel, TProperty>(this IPropertyDescriptor<TModel, TProperty> descriptor) 
            where TModel : ModelBase 
            where TProperty : IEnumerable<ModelBase>
        {
            descriptor.SetValue(DescriptionValue.Editable, typeof(TProperty).ResolveCollectionType());
            return descriptor;
        }
   
        public static IPropertyDescriptor<TModel, TProperty> AllowCreate<TModel, TProperty>(this IPropertyDescriptor<TModel, TProperty> descriptor)
            where TModel : ModelBase
            where TProperty : IEnumerable<ModelBase>
        {
            var i = typeof(TProperty).ResolveCollectionType();
            if (!i.IsInstantiable()) throw new ClassNotInstantiableException(i);
            descriptor.SetValue(DescriptionValue.Creatable, i);
            return descriptor;
        }
     
        public static IPropertyDescriptor<TModel, TProperty> AllowCreate<TModel, TProperty>(
            this IPropertyDescriptor<TModel, TProperty> descriptor, 
            IEnumerable<Type> models) 
            where TModel : ModelBase 
            where TProperty : IEnumerable<ModelBase>
        {
            if (models.Any(p => !p.IsInstantiable())) throw new ClassNotInstantiableException();
            descriptor.SetValue(DescriptionValue.Creatable, models);
            return descriptor;
        }

        public static bool AllowCreate(this IPropertyDescription description)
        {
            return CreatableModels(description).Any();
        }
        public static bool AllowSelect(this IPropertyDescription description)
        {
            return SelectableModels(description).Any();
        }
        public static bool AllowEdit(this IPropertyDescription description)
        {
            return EditableModels(description).Any();
        }

        public static IEnumerable<Type> CreatableModels(this IPropertyDescription description)
        {
            return description[DescriptionValue.Creatable] switch{
                IEnumerable<Type> e => e,
                Type t => new[] { t },
                _ => Type.EmptyTypes
            };
        }
        public static IEnumerable<Type> SelectableModels(this IPropertyDescription description)
        {
            return description[DescriptionValue.Selectable] switch
            {
                IEnumerable<Type> e => e,
                Type t => new[] { t },
                _ => Type.EmptyTypes
            };
        }
        public static IEnumerable<Type> EditableModels(this IPropertyDescription description)
        {
            return description[DescriptionValue.Editable] switch
            {
                IEnumerable<Type> e => e,
                Type t => new[] { t },
                _ => Type.EmptyTypes
            };
        }




        public static IPropertyDescriptor<TModel, TProperty> Source<TModel, TProperty, TElement>(
            this IPropertyDescriptor<TModel, TProperty> descriptor, 
            IQueryable<TElement> source)
            where TModel : ModelBase
            where TElement : ModelBase 
            where TProperty : IEnumerable<TElement> 
        {
            descriptor.SetValue(DescriptionValue.ListSource, source);
            descriptor.SetValue(DescriptionValue.ListSourceType, ListSourceTypeEnum.Queryable);
            return descriptor;
        }

        public static IPropertyDescriptor<TModel, TProperty> Source<TModel, TProperty, TElement>(
            this IPropertyDescriptor<TModel, TProperty> descriptor,
            ICollection<TElement> source)
            where TModel : ModelBase 
            where TElement : ModelBase
            where TProperty : IEnumerable<TElement>
        {
            descriptor.SetValue(DescriptionValue.ListSource, source);
            descriptor.SetValue(DescriptionValue.ListSourceType, ListSourceTypeEnum.Collection);
            return descriptor;
        }

        public static IPropertyDescriptor<TModel, TProperty> Source<TModel, TProperty, TElement>(
            this IPropertyDescriptor<TModel, TProperty> descriptor,
            Func<IEnumerable<TElement>> source) 
            where TModel : ModelBase
            where TElement : ModelBase 
            where TProperty : IEnumerable<TElement>
        {
            descriptor.SetValue(DescriptionValue.ListSource, source);
            descriptor.SetValue(DescriptionValue.ListSourceType, ListSourceTypeEnum.FuncEnumerable);
            return descriptor;
        }

        public static IPropertyDescriptor<TModel, TProperty> Source<TModel, TProperty>(
            this IPropertyDescriptor<TModel, TProperty> descriptor,
            IQueryable<TProperty> source)
            where TModel : ModelBase
            where TProperty : ModelBase
        {
            descriptor.SetValue(DescriptionValue.ListSource, source);
            descriptor.SetValue(DescriptionValue.ListSourceType, ListSourceTypeEnum.Queryable);
            return descriptor;
        }

        public static IPropertyDescriptor<TModel, TProperty> Source<TModel, TProperty>(
            this IPropertyDescriptor<TModel, TProperty> descriptor,
            ICollection<TProperty> source)
            where TModel : ModelBase
            where TProperty : ModelBase
        {
            descriptor.SetValue(DescriptionValue.ListSource, source);
            descriptor.SetValue(DescriptionValue.ListSourceType, ListSourceTypeEnum.Collection);
            return descriptor;
        }

        public static IPropertyDescriptor<TModel, TProperty> Source<TModel, TProperty>(
            this IPropertyDescriptor<TModel, TProperty> descriptor,
            Func<IEnumerable<TProperty>> source)
            where TModel : ModelBase
            where TProperty : ModelBase
        {
            descriptor.SetValue(DescriptionValue.ListSource, source);
            descriptor.SetValue(DescriptionValue.ListSourceType, ListSourceTypeEnum.FuncEnumerable);
            return descriptor;
        }

        public static IEnumerable<ModelBase> ListSource(this IPropertyDescription description)
        {
            return description[DescriptionValue.ListSource] as IEnumerable<ModelBase> 
                ?? Array.Empty<ModelBase>();
        }


        /// <summary>
        /// Validación que comprueba que el objeto no sea <see langword="null"/>.
        /// </summary>
        /// <param name="entity">Entidad a validar.</param>
        /// <param name="prop">Referencia a la propiedad a validar.</param>
        /// <returns>
        /// Una colección de errores de validación si existen problemas, o
        /// una colección vacía si la entidad ha superado todas las
        /// validaciones.
        /// </returns>
        public static IEnumerable<ValidationError> CheckNotNull<T>(T entity, PropertyInfo prop) where T : ModelBase
        {
            if (prop.GetValue(entity) is null) yield return new NullValidationError(prop);
        }
    }
}