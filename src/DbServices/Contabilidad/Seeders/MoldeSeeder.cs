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
            var usd = Proteus.Service<ContabilidadService>()!.Get<Divisa>("en-US")!;
            yield return FakeMolde(usd,1);
            yield return FakeMolde(usd,2);
        }

        private Molde FakeMolde(Divisa usd, int index)
        {
            return new Molde
            {
                Name = $"Catálogo de cuentas de prueba #{index}",
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
                                        $"Caja chica #{index}"
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
                                                $"Banco X #{index}, L #123-456789-012",
                                                $"Banco Y #{index}, L #987-654321-098",
                                            }
                                        },
                                        new CuentaMolde
                                        {
                                            Name = "Moneda extranjera",
                                            SubCuentas =
                                            {
                                                new SubCuentaMolde
                                                {
                                                    Name = $"Banco X #{index}, $ #456-789012-345",
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
                                                $"Edificio X #{index}",
                                                $"Local Y #{index}",
                                                $"Local Z #{index}"
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
                                new CuentaMolde
                                {
                                    Name = "Gastos pagados por anticipado",
                                    Children =
                                    {
                                        new CuentaMolde
                                        {
                                            Name = "Servicios públicos",
                                            SubCuentas =
                                            {
                                                $"ENEE #{index}",
                                                $"SANAA #{index}",
                                                $"Hondutel #{index}"
                                            }
                                        }
                                    }
                                },
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
                                                $"ENEE #{index}",
                                                $"SANAA #{index}",
                                                $"Hondutel #{index}"
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
                                        $"Socio X #{index}",
                                        $"Socio Y #{index}",
                                        $"Socio Z #{index}"
                                    }
                                },
                                new CuentaMolde
                                {
                                    Name = "Aportaciones moneda extranjera",
                                    SubCuentas =
                                    {
                                        new SubCuentaMolde
                                        {
                                            Name = $"Socio X #{index}, $",
                                            DefaultDivisa = usd
                                        },
                                        new SubCuentaMolde
                                        {
                                            Name = $"Socio Y #{index}, $",
                                            DefaultDivisa = usd
                                        },
                                        new SubCuentaMolde
                                        {
                                            Name = $"Socio Z #{index}, $",
                                            DefaultDivisa = usd
                                        }
                                    }
                                }
                            }
                        }
                    }
                },
                Ingresos = new CuentaMolde
                {
                    Name = "Ingresos",
                    Children =
                    {
                        new CuentaMolde
                        {
                            Name = "Ingresos ordinarios",
                            Children =
                            {
                                new CuentaMolde
                                {
                                    Name = "Ingresos por ventas",
                                    SubCuentas =
                                    {
                                        $"Ingresos por ventas #{index}",
                                        $"Devoluciones en ventas #{index}",
                                        $"Descuentos en ventas #{index}"
                                    }
                                },
                            },
                            SubCuentas =
                            {
                                $"Ingresos por servicios #{index}"
                            }
                        },
                        new CuentaMolde
                        {
                            Name = "Ingresos extraordinarios",
                            SubCuentas =
                            {
                                $"Intereses ganados #{index}",
                                $"Ingresos por comisiones #{index}",
                                $"Otros ingresos #{index}",
                                $"Ganancias en venta de activos fijos #{index}",
                            }
                        }
                    }
                },
                Costos = new CuentaMolde
                {
                    Name = "Costos",
                    Children =
                    {
                        new CuentaMolde
                        {
                            Name = "Costos de ventas",
                            SubCuentas =
                            {
                                $"Compra de mercancía #{index}",
                                $"Devoluciones en compras #{index}",
                                $"Descuentos en compras #{index}"
                            }
                        },
                    },
                    SubCuentas =
                    {
                        $"Fletes en compras #{index}"
                    }
                },
                Gastos = new CuentaMolde
                {
                    Name ="Gastos",
                    Children =
                    {
                        new CuentaMolde
                        {
                            Name = "Gastos de ventas",
                            SubCuentas =
                            {
                                $"Gastos de comisiones sobre ventas #{index}",
                                $"Gastos de publicidad #{index}",
                                $"Cuentas incobrables #{index}",
                                $"Gastos de mercadeo #{index}",
                                $"Gastos de transporte #{index}"
                            }
                        },
                        new CuentaMolde
                        {
                            Name = "Gastos administrativos",
                            Children =
                            {
                                new CuentaMolde
                                {
                                    Name = "Servicios públicos",
                                    SubCuentas = {
                                        $"ENEE #{index}",
                                        $"SANAA #{index}",
                                        $"Hondutel #{index}",
                                        $"Millicom Cable #{index}",
                                        $"Cable Color #{index}",
                                        $"Claro #{index}"
                                    }
                                }
                            },
                            SubCuentas =
                            {
                                $"Sueldos #{index}",
                                $"Seguros #{index}",
                                $"Suministros de oficina #{index}",
                                $"Depreciación #{index}",
                                $"Combustible de transporte de personal #{index}",
                                $"Reparación #{index}",
                                $"Organización de la empresa #{index}",
                                $"Instalación de la empresa #{index}",
                                $"Alquiler"
                            }
                        },
                        new CuentaMolde
                        {
                            Name = "Gastos financieros",
                            SubCuentas = {
                                $"Interés sobre préstamos #{index}",
                                $"Comisiones sobre préstamos #{index}",
                                $"Servicios bancarios #{index}"
                            }
                        },
                        new CuentaMolde
                        {
                            Name = "Otros gastos",
                            SubCuentas = {
                                $"Pérdida sobre venta de activos fijos #{index}"
                            }
                        }
                    }
                }
            };
        }
    }
}