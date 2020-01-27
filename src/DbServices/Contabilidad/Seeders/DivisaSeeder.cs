using System.Collections.Generic;
using System.Globalization;
using TheXDS.Proteus.Api;
using TheXDS.Proteus.Models;

namespace TheXDS.Proteus.Seeders
{
    /// <summary>
    /// Define una función de semilla de inicialización para el modelo
    /// <see cref="Divisa"/>.
    /// </summary>
    [SeederFor(typeof(ContabilidadService))]
    public class DivisaSeeder : AsyncDbSeeder<Divisa>
    {
        /// <summary>
        /// Obtiene una colección de entidades a insertar como parte de los
        /// datos iniciales de la tabla del modelo <see cref="Divisa"/>.
        /// </summary>
        /// <returns>
        /// Una enumeración con entidades nuevas a insertar como parte de los
        /// datos iniciales de la tabla del modelo <see cref="Divisa"/>.
        /// </returns>
        protected override IEnumerable<Divisa> GenerateEntities()
        {
            var regs = new[]
            { 
                "en-US",
                "es-ES",
                "en-GB",
                "CN",
                "CN-TW",
                "JP",
                "GT",
                "SV",
                "NI",
                "CR"
            };

            foreach (var j in regs)
            {
                yield return new Divisa
                {
                    Id = j,
                    Name = new RegionInfo(j).CurrencyNativeName
                };
            }
        }
    }
}