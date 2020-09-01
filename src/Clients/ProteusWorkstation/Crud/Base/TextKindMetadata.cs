/*
Copyright © 2017-2020 César Andrés Morgan
Licenciado para uso interno solamente.
*/

using System.Collections.Generic;

namespace TheXDS.Proteus.Crud.Base
{
    internal struct TextKindMetadata
    {
        public int MinLength { get; set; }
        public int MaxLength { get; set; }
        public string? Mask { get; set; }
        public IEnumerable<FileExtension> FileExtensions { get; set; }
    }
}