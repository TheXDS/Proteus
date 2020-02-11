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
            var usd = Proteus.Service<ContabilidadService>()!.Get<Divisa>("en-US");
            yield return new Molde
            {
                Name = "Catálogo de cuentas de prueba",
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
                                new CuentaMolde
                                {
                                    Name = "Bancos",
                                    Children =
                                    {
                                        new CuentaMolde
                                        {
                                            Name = "Moneda nacional",
                                            SubCuentas =
                                            {
                                                "Banco X, L #123-456789-012",
                                                "Banco Y, L #987-654321-098",
                                            }
                                        },
                                        new CuentaMolde
                                        {
                                            Name = "Moneda extranjera",
                                            SubCuentas =
                                            {
                                                new SubCuentaMolde
                                                {
                                                    Name = "Banco X, $ #456-789012-345",
                                                    DefaultDivisa = usd
                                                }
                                            }
                                        }
                                    }
                                },
                                new CuentaMolde
                                {
                                    Name = "Propiedad, planta y equipo",
                                    Children =
                                    {
                                        new CuentaMolde
                                        {
                                            Name = "Eidficios",
                                            SubCuentas =
                                            {
                                                "Edificio X",
                                                "Local Y",
                                                "Local Z"
                                            }
                                        },
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
                                new CuentaMolde
                                {
                                    Name = "Cuentas por pagar",
                                    Children =
                                    {
                                        new CuentaMolde
                                        {
                                            Name = "Servicios públicos",
                                            SubCuentas =
                                            {
                                                "ENEE",
                                                "SANAA",
                                                "Hondutel"
                                            }
                                        }
                                    }
                                }
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
                        new CuentaMolde
                        {
                            Name = "Aportaciones de socios",
                            Children =
                            {
                                new CuentaMolde
                                {
                                    Name = "Aportaciones moneda nacional",
                                    SubCuentas =
                                    {
                                        "Socio X",
                                        "Socio Y",
                                        "Socio Z"
                                    }
                                },
                                new CuentaMolde
                                {
                                    Name = "Aportaciones moneda extranjera",
                                    SubCuentas =
                                    {
                                        new SubCuentaMolde
                                        {
                                            Name = "Socio X, $",
                                            DefaultDivisa = usd
                                        },
                                        new SubCuentaMolde
                                        {
                                            Name = "Socio Y, $",
                                            DefaultDivisa = usd
                                        },
                                        new SubCuentaMolde
                                        {
                                            Name = "Socio Z, $",
                                            DefaultDivisa = usd
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }
    }
}