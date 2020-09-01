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
        #region Descriptores

        public static IPropertyDescriptor<TModel, string> Big<TModel>(this IPropertyDescriptor<TModel, string> descriptor) where TModel : ModelBase
        {
            descriptor.SetValue(DescriptionValue.TextKind, TextKindEnum.Big);
            return descriptor;
        }

        public static IPropertyDescriptor<TModel, string> Rich<TModel>(this IPropertyDescriptor<TModel, string> descriptor) where TModel : ModelBase
        {
            descriptor.SetValue(DescriptionValue.TextKind, TextKindEnum.Rich);
            return descriptor;
        }

        public static IPropertyDescriptor<TModel, string> FilePath<TModel>(this IPropertyDescriptor<TModel, string> descriptor) where TModel : ModelBase
        {
            return FilePath(descriptor, new FileExtension("Todos los archivos", "*") );
        }
        
        public static IPropertyDescriptor<TModel, string> FilePath<TModel>(this IPropertyDescriptor<TModel, string> descriptor, params FileExtension[] fileExtensions) where TModel : ModelBase
        {
            return FilePath(descriptor, fileExtensions.AsEnumerable());
        }
        
        public static IPropertyDescriptor<TModel, string> FilePath<TModel>(this IPropertyDescriptor<TModel, string> descriptor, IEnumerable<FileExtension> fileExtensions) where TModel : ModelBase
        {
            descriptor.SetValue(DescriptionValue.TextKind, TextKindEnum.FilePath);
            return SetTextKindMetadata(descriptor, p => p.FileExtensions, fileExtensions);
        }

        public static IPropertyDescriptor<TModel, string> PicturePath<TModel>(this IPropertyDescriptor<TModel, string> descriptor) where TModel : ModelBase
        {
            descriptor.SetValue(DescriptionValue.TextKind, TextKindEnum.PicturePath);
            return SetTextKindMetadata(descriptor, p => p.FileExtensions, new[] 
            {
                new FileExtension("Todos los archivos de imagen", "png", "jpg", "jpeg", "jpe", "bmp", "gif"),
                new FileExtension("Imagen PNG", "png"),
                new FileExtension("Imagen Jpeg", "jpg", "jpeg", "jpe"),
                new FileExtension("Imagen de mapa de bits", "bmp"),
                new FileExtension("Archivo Gif", "gif")
            });
        }

        public static IPropertyDescriptor<TModel, string> DirectoryPath<TModel>(this IPropertyDescriptor<TModel, string> descriptor) where TModel : ModelBase
        {
            descriptor.SetValue(DescriptionValue.TextKind, TextKindEnum.DirectoryPath);
            return descriptor;
        }

        public static IPropertyDescriptor<TModel, string> Url<TModel>(this IPropertyDescriptor<TModel, string> descriptor) where TModel : ModelBase
        {
            descriptor.SetValue(DescriptionValue.TextKind, TextKindEnum.Url);
            return descriptor;
        }

        public static IPropertyDescriptor<TModel, string> MinLength<TModel>(this IPropertyDescriptor<TModel, string> descriptor, int minLength) where TModel : ModelBase
        {
            return SetTextKindMetadata(descriptor, p => p.MinLength, minLength);
        }

        public static IPropertyDescriptor<TModel, string> MaxLength<TModel>(this IPropertyDescriptor<TModel, string> descriptor, int maxLength) where TModel : ModelBase
        {
            return SetTextKindMetadata(descriptor, p => p.MaxLength, maxLength);
        }

        public static IPropertyDescriptor<TModel, string> Mask<TModel>(this IPropertyDescriptor<TModel, string> descriptor, string mask) where TModel : ModelBase
        {
            return SetTextKindMetadata(descriptor, p => p.Mask, mask);
        }

        private static IPropertyDescriptor<TModel, string> SetTextKindMetadata<TModel, T>(IPropertyDescriptor<TModel, string> descriptor, Expression<Func<TextKindMetadata, T>> property, T metadata) where TModel : ModelBase
        {
            if (!(descriptor[DescriptionValue.TextKindMetadata] is TextKindMetadata m))
            {
                descriptor.SetValue(DescriptionValue.TextKindMetadata, m = new TextKindMetadata());
            }
            ReflectionHelpers.GetProperty(property).SetValue(m, metadata);
            return descriptor;
        }

        #endregion

        #region Descripciones

        public static TextKindEnum? TextKind(this IPropertyDescription description)
        {
            return description[DescriptionValue.TextKind] is TextKindEnum k ? k : (TextKindEnum?)null;
        }

        public static int? TextMinLength(this IPropertyDescription description)
        {
            return description[DescriptionValue.TextKindMetadata] is TextKindMetadata { MinLength: int m } ? m : (int?)null;
        }

        public static int? TextMaxLength(this IPropertyDescription description)
        {
            return description[DescriptionValue.TextKindMetadata] is TextKindMetadata { MaxLength: int m } ? m : (int?)null;
        }

        public static string? TextMask(this IPropertyDescription description)
        {
            return description[DescriptionValue.TextKindMetadata] is TextKindMetadata { Mask: string m } ? m : default;
        }

        public static IEnumerable<FileExtension>? FileExtensions(this IPropertyDescription description)
        {
            return description[DescriptionValue.TextKindMetadata] is TextKindMetadata { FileExtensions: IEnumerable<FileExtension> m } ? m : new[] { FileExtension.AllFiles };
        }

        #endregion
    }
}
