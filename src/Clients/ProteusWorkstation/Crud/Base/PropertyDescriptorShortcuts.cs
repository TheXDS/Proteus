﻿/*
Copyright © 2017-2020 César Andrés Morgan
Licenciado para uso interno solamente.
*/

using TheXDS.Proteus.Models.Base;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using TheXDS.MCART.Attributes;
using TheXDS.MCART.Types.Extensions;
using System.Collections;
using static TheXDS.MCART.Objects;

namespace TheXDS.Proteus.Crud.Base
{
    /// <summary>
    /// Contiene métodos de extensión que proveen de atajos de
    /// configuración para descriptores de propiedades de CRUD.
    /// </summary>
    public static class PropertyDescriptorShortcuts
    {
        #region Atajos de descripción de propiedad

        /// <summary>
        /// Atajo que configura una propiedad como un nombre.
        /// </summary>
        /// <param name="descriptor">Propiedad a configurar.</param>
        /// <param name="label">
        /// Etiqueta opcional a mostrar.
        /// </param>
        /// <returns>
        /// La misma instancia que <paramref name="descriptor"/>.
        /// </returns>
        [Sugar]
        public static IPropertyDescriptor<TModel, string> AsName<TModel>(this IPropertyDescriptor<TModel, string> descriptor, string label) where TModel : ModelBase
        {
            return descriptor.Label(label).Important().NotEmpty();
        }

        /// <summary>
        /// Atajo que configura una propiedad como un nombre.
        /// </summary>
        /// <param name="descriptor">Propiedad a configurar.</param>
        /// <returns>
        /// La misma instancia que <paramref name="descriptor"/>.
        /// </returns>
        [Sugar]
        public static IPropertyDescriptor<TModel, string> AsName<TModel>(this IPropertyDescriptor<TModel, string> descriptor) where TModel : ModelBase => descriptor.AsName("Nombre descriptivo");

        /// <summary>
        /// Muestra una propiedad únicamente en la vista de detalles.
        /// </summary>
        /// <param name="p">Propiedad a configurar.</param>
        /// <returns>
        /// La misma instancia que <paramref name="p"/>.
        /// </returns>
        [Sugar]
        public static IPropertyDescriptor<TModel, TProperty> OnlyInDetails<TModel, TProperty>(this IPropertyDescriptor<TModel, TProperty> p) where TModel : ModelBase
        {
            return p.ShowInDetails().Hidden();
        }

        /// <summary>
        /// Muestra una propiedad únicamente en la vista de detalles.
        /// </summary>
        /// <param name="p">Propiedad a configurar.</param>
        /// <param name="label">
        /// Etiqueta opcional a mostrar.
        /// </param>
        /// <returns>
        /// La misma instancia que <paramref name="p"/>.
        /// </returns>
        [Sugar]
        public static IPropertyDescriptor<TModel, TProperty> OnlyInDetails<TModel, TProperty>(this IPropertyDescriptor<TModel, TProperty> p, string label) where TModel : ModelBase
        {
            return p.ShowInDetails().Label(label).Hidden();
        }

        /// <summary>
        /// Atajo que configura una propiedad como un nombre.
        /// </summary>
        /// <param name="p">Propiedad a configurar.</param>
        /// <param name="label">
        /// Etiqueta opcional a mostrar.
        /// </param>
        /// <param name="icon">Ícono opcional a mostrar.</param>
        /// <returns>
        /// La misma instancia que <paramref name="p"/>.
        /// </returns>
        [Sugar]
        public static IPropertyDescriptor<TModel, TProperty> AsName<TModel, TProperty>(this IPropertyDescriptor<TModel, TProperty> p, string label, string icon) where TModel : ModelBase
        {
            return p.Label(label).Icon(icon).Important();
        }

        /// <summary>
        /// Establece la propiedad como importante, ejecutando
        /// <see cref="IPropertyDescriptor.WatermarkAlwaysVisible()"/>,
        /// <see cref="IPropertyDescriptor.AsListColumn()"/> y 
        /// <see cref="IPropertyDescriptor.ShowInDetails()"/>
        /// </summary>
        /// <param name="p">Propiedad a configurar.</param>
        /// <returns>
        /// La misma instancia que <paramref name="p"/>.
        /// </returns>
        [Sugar]
        public static IPropertyDescriptor<TModel, TProperty> Important<TModel, TProperty>(this IPropertyDescriptor<TModel, TProperty> p) where TModel : ModelBase
        {
            return p.WatermarkAlwaysVisible().ShowInDetails().AsListColumn();
        }

        /// <summary>
        /// Establece la propiedad como importante, ejecutando
        /// <see cref="IPropertyDescriptor.WatermarkAlwaysVisible()"/>,
        /// <see cref="IPropertyDescriptor.AsListColumn()"/> y 
        /// <see cref="IPropertyDescriptor.ShowInDetails()"/>
        /// </summary>
        /// <param name="p">Propiedad a configurar.</param>
        /// <param name="label">Etiqueta a establecer.</param>
        /// <returns>
        /// La misma instancia que <paramref name="p"/>.
        /// </returns>
        [Sugar]
        public static IPropertyDescriptor<TModel, TProperty> Important<TModel, TProperty>(this IPropertyDescriptor<TModel, TProperty> p, string label) where TModel : ModelBase
        {
            return p.WatermarkAlwaysVisible().ShowInDetails().AsListColumn().Label(label);
        }

        /// <summary>
        /// Atajo que configura una propiedad de campo llave.
        /// </summary>
        /// <param name="p">Propiedad a configurar.</param>
        /// <param name="label">Etiqueta a mostrar.</param>
        /// <param name="icon">Ícono opcional a mostrar.</param>
        /// <returns>
        /// La misma instancia que <paramref name="p"/>.
        /// </returns>
        [Sugar]
        public static IPropertyDescriptor<TModel, TProperty> Id<TModel, TProperty>(this IPropertyDescriptor<TModel, TProperty> p, string label, string icon) where TModel : ModelBase
        {
            return p.Label(label).Icon(icon).Important().Required();
        }

        /// <summary>
        /// Atajo que configura una propiedad de campo llave.
        /// </summary>
        /// <param name="p">Propiedad a configurar.</param>
        /// <param name="label">Etiqueta a mostrar.</param>
        /// <returns>
        /// La misma instancia que <paramref name="p"/>.
        /// </returns>
        [Sugar]
        public static IPropertyDescriptor<TModel, TProperty> Id<TModel, TProperty>(this IPropertyDescriptor<TModel, TProperty> p, string label) where TModel : ModelBase
        {
            return p.Id(label, "🗝");
        }

        /// <summary>
        /// Atajo que configura una propiedad de campo llave.
        /// </summary>
        /// <param name="p">Propiedad a configurar.</param>
        /// <returns>
        /// La misma instancia que <paramref name="p"/>.
        /// </returns>
        [Sugar]
        public static IPropertyDescriptor<TModel, TProperty> Id<TModel, TProperty>(this IPropertyDescriptor<TModel, TProperty> p) where TModel : ModelBase
        {
            return p.Id("Id", "🗝");
        }

        /// <summary>
        /// Marca un campo para no ser una cadena vacía.
        /// </summary>
        /// <param name="p">Propiedad a configurar.</param>
        /// <returns>
        /// La misma instancia que <paramref name="p"/>.
        /// </returns>
        [Sugar]
        public static IPropertyDescriptor<TModel, string> NotEmpty<TModel>(this IPropertyDescriptor<TModel, string> p) where TModel : ModelBase
        {
            return p.Required().Validator(CheckNotEmpty);
        }
        
        /// <summary>
        /// Marca una colección para indicar que debe contener al menos un elemento.
        /// </summary>
        /// <param name="p">Propiedad a configurar.</param>
        /// <returns>
        /// La misma instancia que <paramref name="p"/>.
        /// </returns>
        [Sugar]
        public static IPropertyDescriptor<TModel, TProperty> NotEmpty<TModel, TProperty>(this IPropertyDescriptor<TModel, TProperty> p) where TModel : ModelBase where TProperty : IEnumerable<ModelBase>
        {
            return p.Required().Validator(CheckListNotEmpty);
        }

        /// <summary>
        /// Permite agregar múltiples funciones de validación a un mismo
        /// campo.
        /// </summary>
        /// <param name="p">Propiedad a configurar.</param>
        /// <param name="validations">
        /// Funciones de validación a concatenar.
        /// </param>
        /// <returns>
        /// La misma instancia que <paramref name="p"/>.
        /// </returns>
        [Sugar]
        public static IPropertyDescriptor<TModel, TProperty> Validations<TModel, TProperty>(this IPropertyDescriptor<TModel, TProperty> p, params Func<TModel, PropertyInfo, IEnumerable<ValidationError>>[] validations) where TModel : ModelBase
        {
            foreach (var j in validations)
            {
                p.Validator(j);
            }
            return p;
        }

        /// <summary>
        /// Marca un campo como columna de una lista.
        /// </summary>
        /// <param name="p">Propiedad a configurar.</param>
        /// <param name="format">Formato a aplicar al campo.</param>
        /// <returns>
        /// La misma instancia que <paramref name="p"/>.
        /// </returns>
        [Sugar]
        public static IPropertyDescriptor<TModel, TProperty> AsListColumn<TModel, TProperty>(this IPropertyDescriptor<TModel, TProperty> p, string format) where TModel : ModelBase
        {
            p.AsListColumn().Format(format);
            return p;
        }

        /// <summary>
        /// Indica que una lista debe contener controles para la creación
        /// de nuevas entidades de la clase base especificada.
        /// </summary>
        /// <typeparam name="T">
        /// Tipo base de los modelos que deben estar disponibles para la
        /// creación.
        /// </typeparam>
        /// <param name="p">Propiedad a configurar.</param>
        /// <returns>
        /// La misma instancia que <paramref name="p"/>.
        /// </returns>
        [Sugar]
        public static IPropertyDescriptor<TModel, TProperty> AllowCreateAny<TModel, TProperty>(this IPropertyDescriptor<TModel, TProperty> descriptor) where TModel : ModelBase where TProperty : IEnumerable<ModelBase>
        {
            return descriptor.AllowCreate(GetTypes<TProperty>(true));
        }

        /// <summary>
        /// Indica que una lista debe contener controles para la creación
        /// de nuevas entidades de la clase base especificada.
        /// </summary>
        /// <typeparam name="T">
        /// Tipo base de los modelos que deben estar disponibles para la
        /// creación.
        /// </typeparam>
        /// <param name="p">Propiedad a configurar.</param>
        /// <returns>
        /// La misma instancia que <paramref name="p"/>.
        /// </returns>
        [Sugar]
        public static IPropertyDescriptor<TModel, TProperty> CreatableAny<TModel, TProperty>(this IPropertyDescriptor<TModel, TProperty> descriptor) where TModel : ModelBase where TProperty : ModelBase
        {
            return descriptor.Creatable(GetTypes<TProperty>(true));
        }

        /// <summary>
        /// Indica que un campo numérico debe tener valores positivos.
        /// </summary>
        /// <param name="p">Propiedad a configurar.</param>
        /// <returns>
        /// La misma instancia que <paramref name="p"/>.
        /// </returns>
        public static IPropertyDescriptor<TModel, decimal> Positive<TModel>(this IPropertyDescriptor<TModel, decimal> p) where TModel : ModelBase
        {
            return p.Range(0.01m, decimal.MaxValue);
        }

        public static IPropertyDescriptor<TModel, decimal?> Positive<TModel>(this IPropertyDescriptor<TModel, decimal?> p) where TModel : ModelBase
        {
            return p.Range(0.01m, decimal.MaxValue);
        }

        /// <summary>
        /// Indica que un campo numérico debe tener valores positivos.
        /// </summary>
        /// <param name="p">Propiedad a configurar.</param>
        /// <returns>
        /// La misma instancia que <paramref name="p"/>.
        /// </returns>
        public static IPropertyDescriptor<TModel, short> Positive<TModel>(this IPropertyDescriptor<TModel, short> p) where TModel : ModelBase
        {
            return p.Range((short)1, short.MaxValue);
        }

        public static IPropertyDescriptor<TModel, short?> Positive<TModel>(this IPropertyDescriptor<TModel, short?> p) where TModel : ModelBase
        {
            return p.Range((short)1, short.MaxValue);
        }

        /// <summary>
        /// Indica que un campo numérico debe tener valores positivos.
        /// </summary>
        /// <param name="p">Propiedad a configurar.</param>
        /// <returns>
        /// La misma instancia que <paramref name="p"/>.
        /// </returns>
        public static IPropertyDescriptor<TModel, int> Positive<TModel>(this IPropertyDescriptor<TModel, int> p) where TModel : ModelBase
        {
            return p.Range(1, int.MaxValue);
        }

        public static IPropertyDescriptor<TModel, int?> Positive<TModel>(this IPropertyDescriptor<TModel, int?> p) where TModel : ModelBase
        {
            return p.Range(1, int.MaxValue);
        }

        /// <summary>
        /// Indica que un campo numérico debe tener valores positivos.
        /// </summary>
        /// <param name="p">Propiedad a configurar.</param>
        /// <returns>
        /// La misma instancia que <paramref name="p"/>.
        /// </returns>
        public static IPropertyDescriptor<TModel, long> Positive<TModel>(this IPropertyDescriptor<TModel, long> p) where TModel : ModelBase
        {
            return p.Range(1L, long.MaxValue);
        }

        public static IPropertyDescriptor<TModel, long?> Positive<TModel>(this IPropertyDescriptor<TModel, long?> p) where TModel : ModelBase
        {
            return p.Range(1L, long.MaxValue);
        }

        /// <summary>
        /// Indica que un campo numérico debe tener valores positivos.
        /// </summary>
        /// <param name="p">Propiedad a configurar.</param>
        /// <returns>
        /// La misma instancia que <paramref name="p"/>.
        /// </returns>
        public static IPropertyDescriptor<TModel, float> Positive<TModel>(this IPropertyDescriptor<TModel, float> p) where TModel : ModelBase
        {
            return p.Range(float.Epsilon, float.MaxValue);
        }

        public static IPropertyDescriptor<TModel, float?> Positive<TModel>(this IPropertyDescriptor<TModel, float?> p) where TModel : ModelBase
        {
            return p.Range(float.Epsilon, float.MaxValue);
        }

        /// <summary>
        /// Indica que un campo numérico debe tener valores positivos.
        /// </summary>
        /// <param name="p">Propiedad a configurar.</param>
        /// <returns>
        /// La misma instancia que <paramref name="p"/>.
        /// </returns>
        public static IPropertyDescriptor<TModel, double> Positive<TModel>(this IPropertyDescriptor<TModel, double> p) where TModel : ModelBase
        {
            return p.Range(double.Epsilon, double.MaxValue);
        }
        
        public static IPropertyDescriptor<TModel, double?> Positive<TModel>(this IPropertyDescriptor<TModel, double?> p) where TModel : ModelBase
        {
            return p.Range(double.Epsilon, double.MaxValue);
        }






        /// <summary>
        /// Indica que un campo numérico no debe ser igual a cero.
        /// </summary>
        /// <param name="p">Propiedad a configurar.</param>
        /// <returns>
        /// La misma instancia que <paramref name="p"/>.
        /// </returns>
        public static IPropertyDescriptor<TModel, decimal> NonZero<TModel>(this IPropertyDescriptor<TModel, decimal> p) where TModel : ModelBase
        {
            return p.Validations(CheckNotZero);
        }

        /// <summary>
        /// Indica que un campo numérico no debe ser igual a cero.
        /// </summary>
        /// <param name="p">Propiedad a configurar.</param>
        /// <returns>
        /// La misma instancia que <paramref name="p"/>.
        /// </returns>
        public static IPropertyDescriptor<TModel, short> NonZero<TModel>(this IPropertyDescriptor<TModel, short> p) where TModel : ModelBase
        {
            return p.Validations(CheckNotZero);
        }

        /// <summary>
        /// Indica que un campo numérico no debe ser igual a cero.
        /// </summary>
        /// <param name="p">Propiedad a configurar.</param>
        /// <returns>
        /// La misma instancia que <paramref name="p"/>.
        /// </returns>
        public static IPropertyDescriptor<TModel, int> NonZero<TModel>(this IPropertyDescriptor<TModel, int> p) where TModel : ModelBase
        {
            return p.Validations(CheckNotZero);
        }

        /// <summary>
        /// Indica que un campo numérico no debe ser igual a cero.
        /// </summary>
        /// <param name="p">Propiedad a configurar.</param>
        /// <returns>
        /// La misma instancia que <paramref name="p"/>.
        /// </returns>
        public static IPropertyDescriptor<TModel, long> NonZero<TModel>(this IPropertyDescriptor<TModel, long> p) where TModel : ModelBase
        {
            return p.Validations(CheckNotZero);
        }

        /// <summary>
        /// Indica que un campo numérico no debe ser igual a cero.
        /// </summary>
        /// <param name="p">Propiedad a configurar.</param>
        /// <returns>
        /// La misma instancia que <paramref name="p"/>.
        /// </returns>
        public static IPropertyDescriptor<TModel, float> NonZero<TModel>(this IPropertyDescriptor<TModel, float> p) where TModel : ModelBase
        {
            return p.Validations(CheckNotZero);
        }

        /// <summary>
        /// Indica que un campo numérico no debe ser igual a cero.
        /// </summary>
        /// <param name="p">Propiedad a configurar.</param>
        /// <returns>
        /// La misma instancia que <paramref name="p"/>.
        /// </returns>
        public static IPropertyDescriptor<TModel, double> NonZero<TModel>(this IPropertyDescriptor<TModel, double> p) where TModel : ModelBase
        {
            return p.Validations(CheckNotZero);
        }

        /// <summary>
        /// Indica que un campo numérico no debe ser igual a cero.
        /// </summary>
        /// <param name="p">Propiedad a configurar.</param>
        /// <returns>
        /// La misma instancia que <paramref name="p"/>.
        /// </returns>
        public static IPropertyDescriptor<TModel, decimal?> NonZero<TModel>(this IPropertyDescriptor<TModel, decimal?> p) where TModel : ModelBase
        {
            return p.Validations(CheckNotZero);
        }

        /// <summary>
        /// Indica que un campo numérico no debe ser igual a cero.
        /// </summary>
        /// <param name="p">Propiedad a configurar.</param>
        /// <returns>
        /// La misma instancia que <paramref name="p"/>.
        /// </returns>
        public static IPropertyDescriptor<TModel, short?> NonZero<TModel>(this IPropertyDescriptor<TModel, short?> p) where TModel : ModelBase
        {
            return p.Validations(CheckNotZero);
        }

        /// <summary>
        /// Indica que un campo numérico no debe ser igual a cero.
        /// </summary>
        /// <param name="p">Propiedad a configurar.</param>
        /// <returns>
        /// La misma instancia que <paramref name="p"/>.
        /// </returns>
        public static IPropertyDescriptor<TModel, int?> NonZero<TModel>(this IPropertyDescriptor<TModel, int?> p) where TModel : ModelBase
        {
            return p.Validations(CheckNotZero);
        }

        /// <summary>
        /// Indica que un campo numérico no debe ser igual a cero.
        /// </summary>
        /// <param name="p">Propiedad a configurar.</param>
        /// <returns>
        /// La misma instancia que <paramref name="p"/>.
        /// </returns>
        public static IPropertyDescriptor<TModel, long?> NonZero<TModel>(this IPropertyDescriptor<TModel, long?> p) where TModel : ModelBase
        {
            return p.Validations(CheckNotZero);
        }

        /// <summary>
        /// Indica que un campo numérico no debe ser igual a cero.
        /// </summary>
        /// <param name="p">Propiedad a configurar.</param>
        /// <returns>
        /// La misma instancia que <paramref name="p"/>.
        /// </returns>
        public static IPropertyDescriptor<TModel, float?> NonZero<TModel>(this IPropertyDescriptor<TModel, float?> p) where TModel : ModelBase
        {
            return p.Validations(CheckNotZero);
        }

        /// <summary>
        /// Indica que un campo numérico no debe ser igual a cero.
        /// </summary>
        /// <param name="p">Propiedad a configurar.</param>
        /// <returns>
        /// La misma instancia que <paramref name="p"/>.
        /// </returns>
        public static IPropertyDescriptor<TModel, double?> NonZero<TModel>(this IPropertyDescriptor<TModel, double?> p) where TModel : ModelBase
        {
            return p.Validations(CheckNotZero);
        }

        /// <summary>
        /// Indica que un campo de fecha es un campo de marca de tiempo completo.
        /// </summary>
        /// <typeparam name="T">Tipo de descriptor.</typeparam>
        /// <param name="p">Propiedad a configurar.</param>
        /// <returns>
        /// La misma instancia que <paramref name="p"/>.
        /// </returns>
        public static IPropertyDescriptor<TModel, DateTime> Timestamp<TModel>(this IPropertyDescriptor<TModel, DateTime> p) where TModel : ModelBase
        {
            return p.WithTime().Default(DateTime.Now).Label("Fecha de creación");
        }
        
        public static IPropertyDescriptor<TModel, DateTime?> Timestamp<TModel>(this IPropertyDescriptor<TModel, DateTime?> p) where TModel : ModelBase
        {
            return p.WithTime().Default(DateTime.Now).Label("Fecha de creación");
        }

        private static IEnumerable<ValidationError> CheckNotZero(ModelBase m, PropertyInfo prop)
        {
            var v = prop.GetValue(m);
            if (v is null || v.Equals(prop.PropertyType.Default()))
            {
                yield return new NullValidationError(prop);
            }
        }

        #endregion

        #region Bulks de descripción de modelos

        /// <summary>
        /// Describe rápidamente las propiedades de dirección del modelo
        /// actual.
        /// </summary>
        /// <typeparam name="T">
        /// Modelo a describir.
        /// </typeparam>
        /// <param name="descriptor">
        /// Instancia del descriptor de modelos.
        /// </param>
        [Sugar]
        public static void DescribeAddress<T>(this CrudDescriptor<T> descriptor) where T : ModelBase, IAddressable, new()
        {
            descriptor.Property(p => p.Address).Big().Label("Dirección").Icon("🏢").Required().Validator(CheckAddress).ShowInDetails();
            descriptor.Property(p => p.City).Label("Cuidad").Icon("🏙").NotEmpty().ShowInDetails();
            descriptor.Property(p => p.Province).Label("Provincia/Departamento").Icon("🚩").NotEmpty().ShowInDetails();
            descriptor.Property(p => p.Zip).Label("Código Zip").Icon("📮").Nullable().ShowInDetails();
            descriptor.Property(p => p.Country).Label("País").Icon("🏳").NotEmpty().ShowInDetails();
        }

        /// <summary>
        /// Describe rápidamente las propiedades de contacto del modelo
        /// actual.
        /// </summary>
        /// <typeparam name="T">
        /// Modelo a describir.
        /// </typeparam>
        /// <param name="descriptor">
        /// Instancia del descriptor de modelos.
        /// </param>
        [Sugar]
        public static void DescribeContact<T>(this CrudDescriptor<T> descriptor) where T : ModelBase, IContact, new()
        {
            descriptor.Property(p => p.Emails)
                .AllowEdit()
                .AllowCreate()
                .Label("Correos de contacto")
                .Icon("📧")
                .ShowInDetails()
                .Required();

            descriptor.Property(p => p.Phones)
                .AllowEdit()
                .AllowCreate()
                .Label("Teléfonos")
                .Icon("📞")
                .ShowInDetails()
                .Required();
        }

        /// <summary>
        /// Describe rápidamente las propiedades de información de equipo
        /// del modelo actual.
        /// </summary>
        /// <typeparam name="T">
        /// Modelo a describir.
        /// </typeparam>
        /// <param name="descriptor">
        /// Instancia del descriptor de modelos.
        /// </param>
        [Sugar]
        public static void DescribeEstacion<T>(this CrudDescriptor<T> descriptor) where T : EstacionBase, new()
        {
            descriptor.Property(p => p.Id).Id("Nombre del equipo").Default(Environment.MachineName);
            descriptor.Property(p => p.Name).AsName().Default($"Equipo de {Environment.UserName}");
        }

        /// <summary>
        /// Describe una entidad que ofrece representaciones de valores 
        /// absolutos o relativos.
        /// </summary>
        /// <typeparam name="T">
        /// Modelo a describir.
        /// </typeparam>
        /// <param name="descriptor">
        /// Instancia del descriptor de modelos.
        /// </param>
        [Sugar]
        public static void DescribeValuable<T>(this CrudDescriptor<T> descriptor) where T: ModelBase, IValuable, new()
        {
            descriptor.Property(p => p.StaticValue)
                .Range(decimal.Zero, decimal.MaxValue)
                .Nullable()
                .RadioSelectable()
                .Label("Valor estático");

            descriptor.Property(p => p.PercentValue)
                .Range(0f, 1f)
                .Nullable()
                .RadioSelectable()
                .Label("Valor porcentual");
        }

        #endregion

        #region Validaciones personalizadas

        /// <summary>
        /// Valida una dirección.
        /// </summary>
        /// <param name="entity">Entidad a validar.</param>
        /// <param name="prop">Referencia a la propiedad a validar.</param>
        /// <returns>
        /// Una colección de errores de validación si existen problemas, o
        /// una colección vacía si la entidad ha superado todas las
        /// validaciones.
        /// </returns>
        public static IEnumerable<ValidationError> CheckAddress(ModelBase entity, PropertyInfo prop)
        {
            var m = entity as IAddressable ?? throw new InvalidOperationException();
            if (m.Address.IsEmpty()) yield return new ValidationError(prop, "Se necesita una dirección");
            if (m.Address.CountChars(' ', ',') < 3) yield return new ValidationError(prop, "Esa no parece ser una dirección válida.");
        }

        /// <summary>
        /// Validación que comprueba que un campo de tipo <see cref="string"/>
        /// o su representación como una cadena no esté vacío.
        /// </summary>
        /// <param name="entity">Entidad a validar.</param>
        /// <param name="prop">Referencia a la propiedad a validar.</param>
        /// <returns>
        /// Una colección de errores de validación si existen problemas, o
        /// una colección vacía si la entidad ha superado todas las
        /// validaciones.
        /// </returns>
        public static IEnumerable<ValidationError> CheckNotEmpty(ModelBase entity, PropertyInfo prop)
        {
            if (prop.GetValue(entity)?.ToString()?.IsEmpty() ?? true) yield return new ValidationError(prop, "Este campo es requerido.");
        }

        /// <summary>
        /// Validación que comprueba que la lista del campo no se encuentre
        /// vacía.
        /// </summary>
        /// <param name="entity">Entidad a validar.</param>
        /// <param name="prop">Referencia a la propiedad a validar.</param>
        /// <returns>
        /// Una colección de errores de validación si existen problemas, o
        /// una colección vacía si la entidad ha superado todas las
        /// validaciones.
        /// </returns>
        public static IEnumerable<ValidationError> CheckListNotEmpty(ModelBase entity, PropertyInfo prop)
        {
            if (!(prop.GetValue(entity) is IEnumerable c)) yield break;
            if (!c.ToGeneric().Any()) yield return new ValidationError(prop, "Se necesita al menos un elemento en esta colección.");
        }

        #endregion
    }
}