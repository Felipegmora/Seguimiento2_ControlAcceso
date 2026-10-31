using ControlAcceso.Dominio.Entidades.Areas;
using ControlAcceso.Dominio.Entidades.Credenciales;
using ControlAcceso.Dominio.Entidades.Empleados;
using ControlAcceso.Dominio.Entidades.PuntosAcceso;
using ControlAcceso.Dominio.Entidades.RegistrosAcceso;
using Microsoft.EntityFrameworkCore;

namespace ControlAcceso.Persistencia
{
    public sealed class ContextoDatos : DbContext
    {
        public ContextoDatos(DbContextOptions<ContextoDatos> opciones) : base(opciones)
        {
        }

        public DbSet<Area> Areas => Set<Area>();
        public DbSet<Empleado> Empleados => Set<Empleado>();
        public DbSet<Credencial> Credenciales => Set<Credencial>();
        public DbSet<PuntoAcceso> PuntosAcceso => Set<PuntoAcceso>();
        public DbSet<RegistroAcceso> RegistrosAcceso => Set<RegistroAcceso>();

        protected override void OnModelCreating(ModelBuilder constructorModelo)
        {
            base.OnModelCreating(constructorModelo);

            constructorModelo.ApplyConfigurationsFromAssembly(typeof(ContextoDatos).Assembly);
        }
    }
}
