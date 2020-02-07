using System.Collections.Generic;
using TheXDS.Proteus.Api;
using TheXDS.Proteus.Models;
namespace TheXDS.Proteus.Seeders
{
    /// <summary>
    /// Define una función de semilla de inicialización para el modelo
    /// <see cref="Molde"/>.
    /// </summary>
    [SeederFor(typeof(ContabilidadService))]
    public class MoldeSeeder : AsyncDbSeeder<Molde>
    {
        /// <summary>
        /// Obtiene una colección de entidades a insertar como parte de los
        /// datos iniciales de la tabla del modelo <see cref="Molde"/>.
        /// </summary>
        /// <returns>
        /// Una enumeración con entidades nuevas a insertar como parte de los
        /// datos iniciales de la tabla del modelo <see cref="Molde"/>.
        /// </returns>
        protected override IEnumerable<Molde> GenerateEntities()
        {
            yield return new Molde
            {
                Name = "Catálogo de cuentas simple",
                Activo = new CuentaMolde
                {
                    Name = "Activo",
                    Children =
                    {
                        new CuentaMolde
                        {
                            Name = "Activos corrientes",
                            Children =
                            {
                                new CuentaMolde
                                {
                                    Name = "Efectivo y equivalentes", 
                                    SubCuentas =
                                    {
                                        "Caja chica"
                                    }
                                },
                                "Bancos",
                                new CuentaMolde
                                {
                                    Name = "Propiedad, planta y equipo",
                                    Children =
                                    {
                                        "Edificios",
                                        "Terrenos",
                                        "Muebles",
                                        "Equipo de oficina",
                                        "Maquinaria",
                                        "Vehículos"                                        
                                    }
                                },
                                "Inventario",
                                "Gastos pagados por anticipado",
                                "Cuentas por cobrar"
                            }
                        },
                        new CuentaMolde
                        {
                            Name = "Activos no corrientes",
                            Children =
                            {
                                new CuentaMolde
                                {
                                    Name = "Propiedad, planta y equipo",
                                    Children =
                                    {
                                        "Edificios",
                                        "Terrenos",
                                        "Muebles",
                                        "Equipo de oficina",
                                        "Maquinaria",
                                        "Vehículos"
                                    }
                                },
                                "Gastos pagados por anticipado",
                                "Cuentas por cobrar"
                            }
                        }
                    }
                },
                Pasivo = new CuentaMolde
                {
                    Name = "Pasivo",
                    Children =
                    {
                        new CuentaMolde
                        {
                            Name = "Pasivos corrientes",
                            Children =
                            {
                                "Servicios cobrados por anticipado",
                                "Cuentas por pagar"
                            }
                        },
                        new CuentaMolde
                        {
                            Name = "Pasivos no corrientes",
                            Children =
                            {
                                "Servicios cobrados por anticipado",
                                "Cuentas por pagar"
                            }
                        }

                    }
                },
                Patrimonio = new CuentaMolde
                {
                    Name = "Patrimonio",
                    Children =
                    {
                        "Aportaciones de socios"                        
                    }
                }
            };
        }
    }
}