using System.Collections.Generic;
using TheXDS.Proteus.Api;
using TheXDS.Proteus.Models;
namespace TheXDS.Proteus.Seeders
{
    /// <summary>
    /// Define una función de semilla de inicialización para el modelo
    /// <see cref="AccessControlList"/>.
    /// </summary>
    [SeederFor(typeof(ContabilidadService))]
    public class AccessControlListSeeder : AsyncDbSeeder<AccessControlList>
    {
        /// <summary>
        /// Obtiene una colección de entidades a insertar como parte de los
        /// datos iniciales de la tabla del modelo <see cref="AccessControlList"/>.
        /// </summary>
        /// <returns>
        /// Una enumeración con entidades nuevas a insertar como parte de los
        /// datos iniciales de la tabla del modelo <see cref="AccessControlList"/>.
        /// </returns>
        protected override IEnumerable<AccessControlList> GenerateEntities()
        {
            yield return new AccessControlList
            {
                UserId = "root",
                EmpresaDefault = AclValue.Allow,
                EntidadDefault = AclValue.Allow,
                CostCenterDefault = AclValue.Allow
            };
        }
    }
}