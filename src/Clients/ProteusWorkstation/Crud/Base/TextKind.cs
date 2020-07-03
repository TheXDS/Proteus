/*
Copyright © 2017-2020 César Andrés Morgan
Licenciado para uso interno solamente.
*/

namespace TheXDS.Proteus.Crud.Base
{
    /// <summary>
    /// Enumera los tipos de texto que un campo de tipo <see cref="string"/>
    /// puede ser.
    /// </summary>
    public enum TextKind : byte
    {
        /// <summary>
        /// Texto genérico estándar.
        /// </summary>
        Generic,
        /// <summary>
        /// Texto con máscara.
        /// </summary>
        Maskable,
        /// <summary>
        /// Campo de texto de gran tamaño.
        /// </summary>
        Big,
        /// <summary>
        /// Texto enriquecido.
        /// </summary>
        Rich,
        /// <summary>
        /// Ruta de archivo.
        /// </summary>
        FilePath,
        /// <summary>
        /// Ruta de archivo de imagen con vista previa.
        /// </summary>
        PicturePath,
        /// <summary>
        /// Ruta de directorio.
        /// </summary>
        DirectoryPath,
        /// <summary>
        /// Dirección URL.
        /// </summary>
        Url
    }
}