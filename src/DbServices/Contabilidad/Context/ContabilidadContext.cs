using System.Data.Entity;
using TheXDS.Proteus.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheXDS.Proteus.Context
{
    public class ContabilidadContext : ProteusContext
    {
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Entidad> Entidades { get; set; }
        public DbSet<Periodo> Periodos { get; set; }
        public DbSet<CostCenter> CostCenters { get; set; }
        public DbSet<Partida> Partidas { get; set; }
        public DbSet<Movimiento> Movimientos { get; set; }
        public DbSet<SubCuenta> SubCuentas { get; set; }
        public DbSet<Cuenta> Cuentas { get; set; }
        public DbSet<Molde> Moldes { get; set; }
        public DbSet<CuentaMolde> CuentaMoldes { get; set; }
        public DbSet<SubCuentaMolde> SubCuentaMoldes { get; set; }
        public DbSet<Divisa> Divisas { get; set; }
        public DbSet<DocumentRef> DocumentRefs { get; set; }
        public DbSet<DocumentKind> DocumentKinds { get; set; }
        public DbSet<AclEntry> AclEntries { get; set; }
        public DbSet<AccessControlList> AccessControlLists { get; set; }
    }
}