/*
Copyright © 2017-2020 César Andrés Morgan
Licenciado para uso interno solamente.
*/

using System.Collections.Generic;
using System.Linq;
using TheXDS.Proteus.Component.Attributes;

namespace TheXDS.Proteus.Crud.Base
{
    public enum DescriptionValue
    {
        Default,
        UseDefault,
        Label,
        Icon,
        Format,
        [DescriptionDefault(true)] Visible,
        ReadOnly,
        [DescriptionDefault(NullMode.Infer)] Nullability,
        RadioGroup,
        Order,
        Validations,
        Tooltip,
        [DescriptionDefault(true)] WatermarkVisibility,
        [DescriptionDefault(false)] AsListColumn,
        [DescriptionDefault(false)] ShowInDetails,


        [DescriptionDefault(Base.TextKind.Generic)] TextKind,
        TextKindMetadata,


        Range,


        WithTime,


        Creatable,
        Selectable,
        Editable,
        DisplayMember,
        Source
    }

    internal struct TextKindMetadata
    {
        public int MinLength { get; set; }
        public int MaxLength { get; set; }
        public string? Mask { get; set; }
        public IEnumerable<FileExtension> FileExtensions { get; set; }
    }

    public struct FileExtension
    {
        public string Name { get; }

        public IEnumerable<string> Extensions { get; }

        public FileExtension(string name, params string[] extensions) : this(name, extensions.AsEnumerable())
        {
        }

        public FileExtension(string name, IEnumerable<string> extensions)
        {
            Name = name;
            Extensions = extensions;
        }

        public static implicit operator FileExtension(string extension)
        {
            return new FileExtension(extension, new[] { extension });
        }
    }
}