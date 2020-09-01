/*
Copyright © 2017-2020 César Andrés Morgan
Licenciado para uso interno solamente.
*/

using System.Collections.Generic;
using System.Linq;

namespace TheXDS.Proteus.Crud.Base
{
    /// <summary>
    /// Estructura que define una extensión de archivo a incluir cuando se
    /// configura un campo de texto para aceptar archivos.
    /// </summary>
    public struct FileExtension
    {
        /// <summary>
        /// Nombre descriptivo del tipo de archivo.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Colección de extensiones que identifican a un archivo del tipo
        /// especificado.
        /// </summary>
        public IEnumerable<string> Extensions { get; }

        /// <summary>
        /// Inicializa una nueva instancia de la estructura
        /// <see cref="FileExtension"/>.
        /// </summary>
        /// <param name="name">Nombre del tipo de archivo.</param>
        /// <param name="extensions">
        /// Extensiones aceptadas para identificar al tipo de archivo.
        /// </param>
        public FileExtension(string name, params string[] extensions) : this(name, extensions.AsEnumerable())
        {
        }

        /// <summary>
        /// Inicializa una nueva instancia de la estructura
        /// <see cref="FileExtension"/>.
        /// </summary>
        /// <param name="name">Nombre del tipo de archivo.</param>
        /// <param name="extensions">
        /// Extensiones aceptadas para identificar al tipo de archivo.
        /// </param>
        public FileExtension(string name, IEnumerable<string> extensions)
        {
            Name = name;
            Extensions = extensions;
        }

        /// <summary>
        /// Convierte implícitamente un <see cref="string"/> a un
        /// <see cref="FileExtension"/>.
        /// </summary>
        /// <param name="extension">
        /// Cadena con la extensión a convertir.
        /// </param>
        public static implicit operator FileExtension(string extension)
        {
            return new FileExtension(extension, new[] { extension });
        }

        public static FileExtension AllFiles
        {
            get
            {
                return new FileExtension("Todos los archivos", "*");
            }
        }
    }
}