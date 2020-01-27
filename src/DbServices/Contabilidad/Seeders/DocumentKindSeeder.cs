using System.Collections.Generic;
using TheXDS.Proteus.Api;
using TheXDS.Proteus.Models;
namespace TheXDS.Proteus.Seeders
{
    /// <summary>
    /// Define una función de semilla de inicialización para el modelo
    /// <see cref="DocumentKind"/>.
    /// </summary>
    [SeederFor(typeof(ContabilidadService))]
    public class DocumentKindSeeder : AsyncDbSeeder<DocumentKind>
    {
        /// <summary>
        /// Obtiene una colección de entidades a insertar como parte de los
        /// datos iniciales de la tabla del modelo <see cref="DocumentKind"/>.
        /// </summary>
        /// <returns>
        /// Una enumeración con entidades nuevas a insertar como parte de los
        /// datos iniciales de la tabla del modelo <see cref="DocumentKind"/>.
        /// </returns>
        protected override IEnumerable<DocumentKind> GenerateEntities()
        {
            var c = 1;
            var docs = new[]
            { 
                "Factura",
                "Cheque",
                "Recibo",
                "Pagaré",
                "Depósito",
                "Transferencia bancaria"
            };

            foreach (var j in docs)
            {
                yield return new DocumentKind
                {
                    Name = j,
                    Prefix = (byte)c++
                };
            }
        }
    }


}