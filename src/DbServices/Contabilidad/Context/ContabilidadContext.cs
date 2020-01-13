﻿using System.Data.Entity;
using TheXDS.Proteus.Models;

namespace TheXDS.Proteus.Context
{
    public class ContabilidadContext : ProteusContext
    {
        public DbSet<Entidad> Entidades { get; set; }
        public DbSet<Periodo> Periodos { get; set; }
        public DbSet<Partida> Partidas { get; set; }
        public DbSet<Movimiento> Movimientos { get; set; }
        public DbSet<SubCuenta> SubCuentas { get; set; }
        public DbSet<Cuenta> Cuentas { get; set; }
        public DbSet<CuentaMolde> CuentaMoldes { get; set; }
        public DbSet<Molde> Moldes { get; set; }
    }
}