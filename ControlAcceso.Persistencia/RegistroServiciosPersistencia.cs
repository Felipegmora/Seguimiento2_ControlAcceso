using ControlAcceso.Aplicacion.Contratos.Persistencia;
using ControlAcceso.Aplicacion.Contratos.Repositorios;
using ControlAcceso.Persistencia.Repositorios;
using ControlAcceso.Persistencia.UnidadDeTrabajo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ControlAcceso.Persistencia
{
    public static class RegistroServiciosPersistencia
    {
        public static IServiceCollection AgregarServiciosPersistencia(this IServiceCollection servicios, IConfiguration configuracion)
        {
            servicios.AddDbContext<ContextoDatos>(opciones =>
            {
                opciones.UseSqlServer(configuracion.GetConnectionString("ConexionPredeterminada"));
            });

            servicios.AddScoped<IUnidadDeTrabajo, UnidadDeTrabajoEfCore>();

            servicios.AddScoped<IRepositorioAreas, RepositorioAreas>();
            servicios.AddScoped<IRepositorioEmpleados, RepositorioEmpleados>();
            servicios.AddScoped<IRepositorioCredenciales, RepositorioCredenciales>();
            servicios.AddScoped<IRepositorioPuntosAcceso, RepositorioPuntosAcceso>();
            servicios.AddScoped<IRepositorioRegistrosAcceso, RepositorioRegistrosAcceso>();

            return servicios;
        }
    }
}
