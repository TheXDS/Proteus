using System;
using System.Collections.Generic;
using System.Text;
using TheXDS.MCART.Attributes;
using TheXDS.Proteus.Api;
using TheXDS.Proteus.Models;
using TheXDS.Proteus.Seeders;

namespace TheXDS.Proteus
{
    //[Name("Seeder para usuarios de desarrollo")]
    //[SeederFor(typeof(ContabilidadService))]
    //public class SanSeeders : AsyncExternalSeeder<Empresa>
    //{
    //    protected override IEnumerable<Empresa> GenerateEntities()
    //    {
    //        yield return new Empresa
    //        {
    //            Name = "Sociedad Amigos de Los Niños",
    //            Address = "Col. Miraflores",
    //            City = "Tegucigalpa",
    //            Province = "Francisco Morazán",
    //            Country = "Honduras",
    //            RTN = "0801-9000-000000",
    //            Periodos =
    //            {
    //                new Periodo
    //                {
    //                    Timestamp = new DateTime(2019, 11, 1),
    //                    Void = new DateTime(2019, 11, 30)
    //                },
    //                new Periodo
    //                {
    //                    Timestamp = new DateTime(2019, 12, 1),
    //                    Void = new DateTime(2019, 12, 31)
    //                },
    //                new Periodo
    //                {
    //                    Timestamp = new DateTime(2020, 1, 1),
    //                    Void = new DateTime(2019, 1, 31)
    //                },
    //                new Periodo
    //                {
    //                    Timestamp = new DateTime(2020, 2, 1)
    //                }
    //            },
    //            Entidades =
    //            {
    //                new Entidad { Name = "Oficina central" },
    //                new Entidad { Name = "Hogares Nuevo Paraíso" },
    //                new Entidad { Name = "Centro Leopoldina" },
    //                new Entidad { Name = "Hogares Pedro Atala" },
    //                new Entidad { Name = "Flor Azul" },
    //                new Entidad { Name = "Programa de becas" },
    //                new Entidad { Name = "Programa Construcciones" },
    //                new Entidad { Name = "Instituto Reyes Irene" },
    //                new Entidad { Name = "DINAF", CostCenters =
    //                    {
    //                        new CostCenter{ Name= "Centro de Protección Temporal" },
    //                        new CostCenter{ Name = "Casa de Transición" }
    //                    }
    //                },
    //                new Entidad { Name = "Dirección general" },
    //                new Entidad { Name = "Hogar Santiago Apóstol" },
    //                new Entidad { Name = "General" },
    //            },
    //            Activo = new Cuenta
    //            {
    //                Name = "Activo",
    //                Prefix = 1,
    //                Children =
    //                {
    //                    new Cuenta
    //                    {
    //                        Name = "Activo Corriente",
    //                        Prefix = 1,
    //                        Children =
    //                        {
    //                            new Cuenta
    //                            {
    //                                Name = "Efectivo y equivalentes de efectivo",
    //                                Prefix = 1,
    //                                Children =
    //                                {
    //                                    new Cuenta
    //                                    {
    //                                        Name = "Caja",
    //                                        Prefix = 1,
    //                                        SubCuentas =
    //                                        {
    //                                            new SubCuenta { Name = "Caja Administración (GNR)", Prefix = 1 },
    //                                            new SubCuenta { Name = "Caja (OCE)", Prefix = 2 },
    //                                            new SubCuenta { Name = "Caja General Farmacia (CSR)", Prefix = 3 },
    //                                            new SubCuenta { Name = "Caja General Emergencia (CSR)", Prefix = 4 }
    //                                        }
    //                                    },
    //                                    new Cuenta
    //                                    {
    //                                        Name = "Caja Chica",
    //                                        Prefix = 2,
    //                                        SubCuentas =
    //                                        {
    //                                            new SubCuenta { Name = "Caja Chica (GNR)", Prefix = 1 },
    //                                            new SubCuenta { Name = "Caja Chica (OCE)", Prefix = 2 },
    //                                            new SubCuenta { Name = "Caja Chica (HNP) Nuevo Paraíso", Prefix = 3 },
    //                                            new SubCuenta { Name = "Caja Chica (HFA) Hogar Flor Azul", Prefix = 4 },
    //                                            new SubCuenta { Name = "Caja Chica (HPA) Hogares Pedro Atala", Prefix = 5 },
    //                                            new SubCuenta { Name = "Caja Chica (RIV) Reyes Irene Valenzuela", Prefix = 6 },
    //                                            new SubCuenta { Name = "Caja Chica (CSR) Clínica Santa Rosa de Lima", Prefix = 7 },
    //                                            new SubCuenta { Name = "Caja Chica (CSR) Clínica Santa Rosa de Lima Materno", Prefix = 8 },
    //                                            new SubCuenta { Name = "Caja Chica (SAP) Santiago Apóstol", Prefix = 9 },
    //                                            new SubCuenta { Name = "Caja Chica (CST) Casas de Transición Dios en Casa", Prefix = 10 },
    //                                            new SubCuenta { Name = "Caja Chica (CST) Casas de Transición Varones", Prefix = 11 },
    //                                            new SubCuenta { Name = "Caja Chica (DNF) DINAF", Prefix = 12 },
    //                                            new SubCuenta { Name = "Caja Chica (CLL) Centro Leopoldina Leal", Prefix = 13 },
    //                                            new SubCuenta { Name = "Caja Chica (DNF) DINAF - Centro de Protección Temporal", Prefix = 14 },
    //                                        }
    //                                    },
    //                                    new Cuenta
    //                                    {
    //                                        Name = "FONDOS DE EMERGENCIA",
    //                                        Prefix = 3,
    //                                        SubCuentas =
    //                                        {
    //                                            new SubCuenta { Name = "Fondo de Emergencia (GNR)", Prefix = 1 },
    //                                            new SubCuenta { Name = "Fondo de Emergencia (OCE)", Prefix = 2 },
    //                                            new SubCuenta { Name = "Fondo de Emergencia (HNP)", Prefix = 3 },
    //                                            new SubCuenta { Name = "Fondo de Emergencia (HFA)", Prefix = 4 },
    //                                            new SubCuenta { Name = "Fondo de Emergencia (HPA)", Prefix = 5 },
    //                                            new SubCuenta { Name = "Fondo de Emergencia (RIV)", Prefix = 6 },
    //                                            new SubCuenta { Name = "Fondo de Emergencia (CSR)", Prefix = 7 },
    //                                            new SubCuenta { Name = "Fondo de Emergencia (HSA)", Prefix = 8 },
    //                                            new SubCuenta { Name = "Fondo de Emergencia (SAP) Santiago Apóstol", Prefix = 9 },
    //                                            new SubCuenta { Name = "Fondo de Emergencia (CST) Casas de Transición Dios en Casa", Prefix = 10 },
    //                                            new SubCuenta { Name = "Fondo de Emergencia (CST) Casas de Transición Varones", Prefix = 11 },
    //                                            new SubCuenta { Name = "Fondo de Emergencia (DNF) DINAF", Prefix = 12 },
    //                                            new SubCuenta { Name = "Fondo de Emergencia (CLL) Centro Leopoldina Leal", Prefix = 13 },
    //                                            new SubCuenta { Name = "Fondo de Emergencia (DNF) DINAF - Centro de Protección Temporal", Prefix = 14 },
    //                                        }
    //                                    },
    //                                    new Cuenta
    //                                    {
    //                                        Name = "Bancos",
    //                                        Prefix = 4,
    //                                        Children =
    //                                        {
    //                                            new Cuenta
    //                                            {
    //                                                Name = "Bancos Moneda Nacional (Cuenta de Cheques)",
    //                                                Prefix = 1,
    //                                                SubCuentas =
    //                                                {
    //                                                    new SubCuenta { Name = "Cuenta No. 911278719 (GNR)", Prefix = 1 },
    //                                                    new SubCuenta { Name = "Cuenta No. 911278717 (OCE)", Prefix = 2 },
    //                                                    new SubCuenta { Name = "Cuenta No. 911278714 FOHC (HNP)", Prefix = 3 },
    //                                                    new SubCuenta { Name = "Cuenta No. 911278706 (HNP)", Prefix = 4 },
    //                                                    new SubCuenta { Name = "Cuenta No. 911278713 (HFA)", Prefix = 5 },
    //                                                    new SubCuenta { Name = "Cuenta No. 911278702 (HPA)", Prefix = 6 },
    //                                                    new SubCuenta { Name = "Cuenta No. 911278705 Construccion (HPA)", Prefix = 7 },
    //                                                    new SubCuenta { Name = "Cuenta No. 911278701 FOHC (RIV)", Prefix = 8 },
    //                                                    new SubCuenta { Name = "Cuenta No. 911278703 HCRF (RIV)", Prefix = 9 },
    //                                                    new SubCuenta { Name = "Cuenta No. 730172761 Brucke (RIV) ", Prefix = 10 },
    //                                                    new SubCuenta { Name = "Cuenta No. 911278718 (CSR)", Prefix = 11 },
    //                                                    new SubCuenta { Name = "Cuenta No. 730211771 CMI (CSR)", Prefix = 12 },
    //                                                    new SubCuenta { Name = "Cuenta No. 911278713 (SAP)", Prefix = 13 },
    //                                                    new SubCuenta { Name = "Cuenta No. 730067061 (BCS)", Prefix = 14 },
    //                                                    new SubCuenta { Name = "Cuenta No. 911278712 (CST)", Prefix = 15 },
    //                                                    new SubCuenta { Name = "Cuenta No. 730258591 (CNP)", Prefix = 16 },
    //                                                    new SubCuenta { Name = "Cuenta No. 911278716 (DNF)", Prefix = 17 },
    //                                                    new SubCuenta { Name = "Cuenta Banadesa No. 01-001-001327-0 (DINAF)", Prefix = 18 },
    //                                                    new SubCuenta { Name = "Cuenta No. 730085161 (CLL)", Prefix = 19 },
    //                                                    new SubCuenta { Name = "Cuenta No. 911278715 (DG)", Prefix = 20 },
    //                                                    new SubCuenta { Name = "Cuenta Banadesa No. 01-001-002091-7 (DINAF) Centro Temporal", Prefix = 21 }
    //                                                }
    //                                            },
    //                                            new Cuenta
    //                                            {
    //                                                Name = "Bancos Moneda Extranjera (Cuentas de Cheques)",
    //                                                Prefix = 2,
    //                                                SubCuentas =
    //                                                {
    //                                                    new SubCuenta { Name = "Cuenta No. 911278711 (GNR)", Prefix = 1 },
    //                                                    new SubCuenta { Name = "Cuenta No. 727525061 (GNR)", Prefix = 2 },
    //                                                    new SubCuenta { Name = "Cuenta No. 730249641 BRUCKE (RIV)", Prefix = 3 }
    //                                                }
    //                                            },
    //                                            new Cuenta
    //                                            {
    //                                                Name = "Bancos Moneda Nacional (Cuenta de Ahorros)",
    //                                                Prefix = 3,
    //                                                SubCuentas =
    //                                                {
    //                                                    new SubCuenta { Name = "Cuenta No. 50002491 RF 4767 (CSR) Aportaciones", Prefix = 1 },
    //                                                    new SubCuenta { Name = "Cuenta No. 50002491 RF 4768 (CSR) Consulta", Prefix = 2 },
    //                                                    new SubCuenta { Name = "Cuenta No. 50002491 RF 4769 (CSR) Medicamentos", Prefix = 3 },
    //                                                    new SubCuenta { Name = "Cuenta No. 50002491 RF 4770 (CSR) Especial", Prefix = 4 },
    //                                                }
    //                                            },

    //                                        }
    //                                    },
    //                                    new Cuenta
    //                                    {
    //                                        Name = "Inversiones Financieras Equivalentes de Efectivo",
    //                                        Prefix = 5,
    //                                        Children=
    //                                        {
    //                                            new Cuenta
    //                                            {
    //                                                Name = "Depósitos a Plazo",
    //                                                Prefix = 1,
    //                                                SubCuentas =
    //                                                {
    //                                                    new SubCuenta { Name = "Banco BAC, Cta 727887651 (GNR) Pensiones Honduras", Prefix = 1 },
    //                                                }
    //                                            },


    //                                        }
    //                                    },



    //                                }
    //                            },
    //                            new Cuenta
    //                            {
    //                                Name = "Cuentas Por Cobrar",
    //                                Prefix = 2,
    //                                Children = 
    //                                {
    //                                    new Cuenta
    //                                    {
    //                                        Name = "Clientes",
    //                                        Prefix = 1,
    //                                        SubCuentas =
    //                                        {
    //                                            new SubCuenta { Name = "Cuentas por cobrar Clientes (GNR)", Prefix = 1 },
    //                                            new SubCuenta { Name = "Cuentas por cobrar Clientes (OCE)", Prefix = 2 },
    //                                            new SubCuenta { Name = "Cuentas por cobrar Clientes (HNP)", Prefix = 3 },
    //                                            new SubCuenta { Name = "Cuentas por cobrar Clientes (HFA)", Prefix = 4 },
    //                                            new SubCuenta { Name = "Cuentas por cobrar Clientes (HPA)", Prefix = 5 },
    //                                            new SubCuenta { Name = "Cuentas por cobrar Clientes (RIV)", Prefix = 6 },
    //                                            new SubCuenta { Name = "Cuentas por cobrar Clientes (CSR)", Prefix = 7 },
    //                                            new SubCuenta { Name = "Cuentas por cobrar Clientes (SAP)", Prefix = 8 },
    //                                            new SubCuenta { Name = "Cuentas por cobrar Clientes (BCS)", Prefix = 9 },
    //                                            new SubCuenta { Name = "Cuentas por cobrar Clientes (CST)", Prefix = 10 },
    //                                            new SubCuenta { Name = "Cuentas por cobrar Clientes (CNP)", Prefix = 11 },
    //                                            new SubCuenta { Name = "Cuentas por cobrar Clientes (DNF)", Prefix = 12 },
    //                                            new SubCuenta { Name = "Cuentas por cobrar Clientes (CLL)", Prefix = 13 },
    //                                            new SubCuenta { Name = "Cuentas por cobrar Clientes (DNF) Centro Temporal", Prefix = 14 },
    //                                            new SubCuenta { Name = "Cuentas por cobrar Clientes (GKNP)", Prefix = 15 }
    //                                        }
    //                                    },
    //                                    new Cuenta
    //                                    {
    //                                        Name = "CUENTAS POR COBRAR EMPLEADOS",
    //                                        Prefix = 2,
    //                                        SubCuentas =
    //                                        {
    //                                            new SubCuenta { Name = "Cuentas por cobrar Empleados (GNR)", Prefix = 1 },
    //                                            new SubCuenta { Name = "Cuentas por cobrar Empleados (OCE)", Prefix = 2 },
    //                                            new SubCuenta { Name = "Cuentas por cobrar Empleados (HNP)", Prefix = 3 },
    //                                            new SubCuenta { Name = "Cuentas por cobrar Empleados (HFA)", Prefix = 4 },
    //                                            new SubCuenta { Name = "Cuentas por cobrar Empleados (HPA)", Prefix = 5 },
    //                                            new SubCuenta { Name = "Cuentas por cobrar Empleados (RIV)", Prefix = 6 },
    //                                            new SubCuenta { Name = "Cuentas por cobrar Empleados (CSR)", Prefix = 7 },
    //                                            new SubCuenta { Name = "Cuentas por cobrar Empleados (SAP)", Prefix = 8 },
    //                                            new SubCuenta { Name = "Cuentas por cobrar Empleados (BCS)", Prefix = 9 },
    //                                            new SubCuenta { Name = "Cuentas por cobrar Empleados (CST)", Prefix = 10 },
    //                                            new SubCuenta { Name = "Cuentas por cobrar Empleados (CST) Dios En Casa", Prefix = 11 },
    //                                            new SubCuenta { Name = "Cuentas por cobrar Empleados (CNP)", Prefix = 12 },
    //                                            new SubCuenta { Name = "Cuentas por cobrar Empleados (DNF)", Prefix = 13 },
    //                                            new SubCuenta { Name = "Cuentas por cobrar Empleados (CLL)", Prefix = 14 },
    //                                            new SubCuenta { Name = "Cuentas por cobrar Empleados (DNF) Centro Temporal", Prefix = 15 },
    //                                            new SubCuenta { Name = "Cuentas por cobrar Empleados (GKNP)", Prefix = 16 }
    //                                        }
    //                                    },
    //                                    new Cuenta
    //                                    {
    //                                        Name = "Cuentas por cobrar entre proyectos",
    //                                        Prefix = 3,
    //                                        SubCuentas =
    //                                        {
    //                                            new SubCuenta { Name = "Cuentas por cobrar Entre Proyectos (GNR)", Prefix = 1 },
    //                                            new SubCuenta { Name = "Cuentas por cobrar Entre Proyectos (OCE)", Prefix = 2 },
    //                                            new SubCuenta { Name = "Cuentas por cobrar Entre Proyectos (HNP)", Prefix = 3 },
    //                                            new SubCuenta { Name = "Cuentas por cobrar Entre Proyectos (HFA)", Prefix = 4 },
    //                                            new SubCuenta { Name = "Cuentas por cobrar Entre Proyectos (HPA)", Prefix = 5 },
    //                                            new SubCuenta { Name = "Cuentas por cobrar Entre Proyectos (RIV)", Prefix = 6 },
    //                                            new SubCuenta { Name = "Cuentas por cobrar Entre Proyectos (CSR)", Prefix = 7 },
    //                                            new SubCuenta { Name = "Cuentas por cobrar Entre Proyectos (SAP)", Prefix = 8 },
    //                                            new SubCuenta { Name = "Cuentas por cobrar Entre Proyectos (BCS)", Prefix = 9 },
    //                                            new SubCuenta { Name = "Cuentas por cobrar Entre Proyectos (CST)", Prefix = 10 },
    //                                            new SubCuenta { Name = "Cuentas por cobrar Entre Proyectos (CST) Dios En Casa", Prefix = 11 },
    //                                            new SubCuenta { Name = "Cuentas por cobrar Entre Proyectos (CNP)", Prefix = 12 },
    //                                            new SubCuenta { Name = "Cuentas por cobrar Entre Proyectos (DNF)", Prefix = 13 },
    //                                            new SubCuenta { Name = "Cuentas por cobrar Entre Proyectos (CLL)", Prefix = 14 },
    //                                            new SubCuenta { Name = "Cuentas por cobrar Entre Proyectos (DNF) Centro Temporal", Prefix = 15 },
    //                                            new SubCuenta { Name = "Cuentas por cobrar Entre Proyectos (GKNP)", Prefix = 16 }
    //                                        }
    //                                    },
    //                                    new Cuenta
    //                                    {
    //                                        Name = "Intereses por Cobrar de Inversiones Depositos a Plazo",
    //                                        Prefix = 4,
    //                                    },
    //                                    new Cuenta
    //                                    {
    //                                        Name = "Otros Intereses y Dividendos por Cobrar",
    //                                        Prefix = 5,
    //                                    },
    //                                    new Cuenta
    //                                    {
    //                                        Name = "Otras Cuentas por Cobrar",
    //                                        Prefix = 6,
    //                                        SubCuentas =
    //                                        {
    //                                            new SubCuenta
    //                                            {
    //                                                Name = "Otras Cuentas por Cobrar (GNR)",
    //                                                Prefix = 1,
    //                                            },
    //                                            new SubCuenta
    //                                            {
    //                                                Name = "Otras Cuentas por Cobrar (OCE)",
    //                                                Prefix = 2,
    //                                            },
    //                                            new SubCuenta
    //                                            {
    //                                                Name = "Otras Cuentas por Cobrar (CSR)",
    //                                                Prefix = 3,
    //                                            }
    //                                        }
    //                                    },
    //                                    new Cuenta
    //                                    {
    //                                        Name = "Deudores Varios",
    //                                        Prefix = 7,
    //                                        SubCuentas =
    //                                        {
    //                                            new SubCuenta
    //                                            {
    //                                                Name = "Deudores Varios (GNR)",
    //                                                Prefix = 1,
    //                                            },
    //                                            new SubCuenta
    //                                            {
    //                                                Name = "Deudores Varios (OCE)",
    //                                                Prefix = 2,
    //                                            }
    //                                        }
    //                                    },
    //                                }
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //        };
    //    }
    //}
}
