/*
Copyright © 2017-2020 César Andrés Morgan
Licenciado para uso interno solamente.
*/

using System;
using System.Linq;
using System.Threading.Tasks;
using TheXDS.MCART.Attributes;
using TheXDS.Proteus.Api;
using TheXDS.Proteus.Component;
using TheXDS.Proteus.Models;
using TheXDS.Proteus.Models.Misc;

namespace TheXDS.Proteus.Seeders
{
    [Name("Seeder de demostración de facturación")]
    [Description("Permite semillar una base de datos de prueba para realizar demostraciones del sistema de facturación.")]
    [SeederFor(typeof(FacturaService))]
    public class FacturaDemoSeeder : IAsyncDbSeeder
    {
        public Task<DetailedResult> SeedAsync(IFullService service, IStatusReporter? reporter)
        {            
            var prods = new Producto[9];
            var svc = new Servicio
            {
                Id = "1001",
                Name = "Lavado de cabello",
                Precio = 200,
            };
            var cat1 = new FacturableCategory
            {
                Name = "Canasta básica",
                Isv = 0f,
                Children =
                {
                    (prods[0] = new Producto
                    {
                        Id="0001",
                        Description="Cartón de huevos blancos con extra calcio.",
                        ExpiryDays=15,
                        Name="Huevos",
                        Precio=45m,
                        StockMax=100,
                        StockMin=5,                        
                    }),
                    (prods[1] = new Producto
                    {
                        Id="0002",
                        Description = "Paquete de leche de vaca, presentación de 1 litro.",
                        ExpiryDays = 10,
                        Name="Leche",
                        Precio=27m,
                        StockMax=100,
                        StockMin=5,
                    }),
                    (prods[2] = new Producto
                    {
                        Id="0003",
                        Description = "Paquete de arroz pre-cocido. Bolsa de 2.54 libras (1 Kg)",
                        ExpiryDays = 45,
                        Name="Arroz",
                        Precio=35m,
                        StockMax=100,
                        StockMin=5,
                    })
                }
            };
            var cat2 = new FacturableCategory
            {
                Name = "Artículos",
                Isv = 15f,
                Children =
                {
                    (prods[3] = new Producto
                    {
                        Id="0004",
                        Description="Shampoo anti caída. Botella de 800 mL",
                        Name="Shampoo",
                        Precio=150m,
                        StockMax=100,
                        StockMin=5,
                    }),
                    (prods[4] = new Producto
                    {
                        Id="0005",
                        Description = "Papel higiénico extra suave, 500 hojas. Paquete de 12 rollos.",
                        Name="Papel higiénico",
                        Precio=120m,
                        StockMax=100,
                        StockMin=5,
                    }),
                    (prods[5] = new Producto
                    {
                        Id="0006",
                        Description = "Jabón de baño con aroma a miel. Paquete de 6 unidades",
                        Name="Jabón",
                        Precio=45m,
                        StockMax=100,
                        StockMin=5,
                    }),
                    new Paquete
                    {
                        Id = "2001",
                        Children =
                        {
                            svc,
                            prods[3]
                        },
                        Name="Promoción Cabello sano",
                        Precio=250,
                        ValidFrom = DateTime.Today - TimeSpan.FromDays(5),
                        Void = DateTime.Today + TimeSpan.FromDays(30),
                    }
                }
            };
            var cat3 = new FacturableCategory
            {
                Name = "Bebidas alcohólicas",
                Isv = 18f,
                Children =
                {
                    (prods[6] = new Producto
                    {
                        Id="0007",
                        Description="Six-pack Cerveza nacional",
                        Name="Cerveza",
                        ExpiryDays = 180,
                        Precio=90m,
                        StockMax=100,
                        StockMin=5,
                    }),
                    (prods[7] = new Producto
                    {
                        Id="0008",
                        Description = "Botella de vino de reserva. Presentación de 1L",
                        Name="Vino",
                        Precio=220m,
                        StockMax=100,
                        StockMin=5,
                    }),
                    (prods[8] = new Producto
                    {
                        Id = "0009",
                        Description = "Botella de ron añejado. Presentación de 800 mL",
                        Name = "Ron",
                        Precio = 245m,
                        StockMax = 100,
                        StockMin = 5,
                    })
                }
            };
            var cat4 = new FacturableCategory
            {
                Name = "Servicios",
                Isv = 15f,
                Children =
                {
                    svc
                }
            };
            var bodega = new Bodega
            {
                Name = "Bodega principal",                
            };
            var bod2 = new Bodega
            {
                Name = "Bodega de reserva"
            };
            var bod3 = new Bodega
            {
                Name = "Alcoholes"
            };
            var est = new Estacion
            {
                Id = Environment.MachineName,
                MinFacturasAlert = 100,
                Name = "POS de demostración",
                PrintDriver = PrintDriverSource.GetDrivers().ToList().FirstOrDefault()?.DriverGuid,
                Printer = PrinterSource.GetPrinters().ToList().FirstOrDefault()?.Printer,
                Bodegas = { bodega, bod2, bod3 },
                Entidad = new Entidad
                {
                    Id = "TLFK",
                    Name = "Tiendas \"La Fake\"",
                    Banner = "Tienda fictícia, pero ofertas reales!",
                    Address = "6th ave, 12 & 13 Pinoa street, #206",
                    City = "San Fierro",
                    Province = "San Andreas",
                    Zip = "10001",
                    Country = "Narnia",
                    Emails =
                    {
                        new Email { Address = "sales@lafake.com" },
                        new Email { Address = "help@lafake.com" }
                    },
                    Phones =
                    {
                        new Phone { Number = "5555-1234" },
                        new Phone { Number = "2222-1234" }
                    }
                },
                SecondScreen = 2
            };

            foreach (var j in prods)
            {
                bodega.Batches.Add(new Batch
                {
                    Item = j,
                    Lote = new Lote
                    {
                        Manufactured = DateTime.Now - TimeSpan.FromDays(3)
                    },
                    Qty = 50,
                    Timestamp = DateTime.Now - TimeSpan.FromDays(1.5)
                });
            }

            service.Add(
                new Cajero
                {
                    OptimBalance = 2000,
                    UserId = "root"
                },
                new Cajero
                {
                    OptimBalance = 2000,
                    UserId = "demo"
                }
            );
            service.Add(new Cai
            {
                Id = "A1B2C3-D4E5F6-G7H8I9-J0K1L2-M3N4O5-P6",
                IsDeleted = false,
                Rangos = {
                    new CaiRango
                    {
                        AssignedTo = est,
                        NumCaja = 1,
                        NumDocumento = 1,
                        NumLocal = 1,
                        RangoFinal = 1000,
                        RangoInicial = 1
                    }
                },
                Timestamp = DateTime.Today - TimeSpan.FromDays(5),
                Void = (DateTime.Today + TimeSpan.FromDays(360.25)).Date
            });
            service.Add(cat1, cat2, cat3, cat4);
            return service.SaveAsync();
        }

        public async Task<bool> ShouldRunAsync(IReadAsyncService service, IStatusReporter? reporter)
        {
            var r = await service.AnyAsync<Cai>();
            return !r;
        }
    }
}