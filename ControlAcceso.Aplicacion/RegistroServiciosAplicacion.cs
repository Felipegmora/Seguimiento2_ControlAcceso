using ControlAcceso.Aplicacion.CasosDeUso.Areas.Consultas;
using ControlAcceso.Aplicacion.CasosDeUso.Areas.Dtos;
using ControlAcceso.Aplicacion.CasosDeUso.Credenciales.Comandos;
using ControlAcceso.Aplicacion.CasosDeUso.Credenciales.Consultas;
using ControlAcceso.Aplicacion.CasosDeUso.Credenciales.Dtos;
using ControlAcceso.Aplicacion.CasosDeUso.Empleados.Comandos;
using ControlAcceso.Aplicacion.CasosDeUso.Empleados.Consultas;
using ControlAcceso.Aplicacion.CasosDeUso.Empleados.Dtos;
using ControlAcceso.Aplicacion.CasosDeUso.PuntosAcceso.Comandos;
using ControlAcceso.Aplicacion.CasosDeUso.PuntosAcceso.Consultas;
using ControlAcceso.Aplicacion.CasosDeUso.PuntosAcceso.Dtos;
using ControlAcceso.Aplicacion.CasosDeUso.RegistrosAcceso.Comandos;
using ControlAcceso.Aplicacion.CasosDeUso.RegistrosAcceso.Consultas;
using ControlAcceso.Aplicacion.CasosDeUso.RegistrosAcceso.Dtos;
using ControlAcceso.Aplicacion.Utilidades.Mediador;
using ControlAcceso.Aplicacion.Utilidades.Paginacion;
using ControlAcceso.Aplicacion.Utilidades.Resultados;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ControlAcceso.Aplicacion
{
    public static class RegistroServiciosAplicacion
    {
        public static IServiceCollection AgregarServiciosAplicacion(this IServiceCollection servicios)
        {
            servicios.AddScoped<IMediador, MediadorSimple>();

            servicios.AddValidatorsFromAssembly(typeof(RegistroServiciosAplicacion).Assembly);

            servicios.AddScoped<IManejadorPeticion<ListarAreasConsulta, Resultado<List<AreaDto>>>, ListarAreasManejador>();

            servicios.AddScoped<IManejadorPeticion<CrearEmpleadoComando, Resultado<Guid>>, CrearEmpleadoManejador>();
            servicios.AddScoped<IManejadorPeticion<ActualizarEmpleadoComando, Resultado>, ActualizarEmpleadoManejador>();
            servicios.AddScoped<IManejadorPeticion<DesactivarEmpleadoComando, Resultado>, DesactivarEmpleadoManejador>();
            servicios.AddScoped<IManejadorPeticion<ObtenerEmpleadoPorIdConsulta, Resultado<EmpleadoDto>>, ObtenerEmpleadoPorIdManejador>();
            servicios.AddScoped<IManejadorPeticion<ListarEmpleadosConsulta, Resultado<RespuestaPaginada<EmpleadoDto>>>, ListarEmpleadosManejador>();

            servicios.AddScoped<IManejadorPeticion<AsignarCredencialComando, Resultado<Guid>>, AsignarCredencialManejador>();
            servicios.AddScoped<IManejadorPeticion<BloquearCredencialComando, Resultado>, BloquearCredencialManejador>();
            servicios.AddScoped<IManejadorPeticion<RevocarCredencialComando, Resultado>, RevocarCredencialManejador>();
            servicios.AddScoped<IManejadorPeticion<ObtenerCredencialPorCodigoConsulta, Resultado<CredencialDto>>, ObtenerCredencialPorCodigoManejador>();
            servicios.AddScoped<IManejadorPeticion<ListarCredencialesPorEmpleadoConsulta, Resultado<List<CredencialDto>>>, ListarCredencialesPorEmpleadoManejador>();

            servicios.AddScoped<IManejadorPeticion<RegistrarPuntoAccesoComando, Resultado<Guid>>, RegistrarPuntoAccesoManejador>();
            servicios.AddScoped<IManejadorPeticion<CambiarEstadoPuntoAccesoComando, Resultado>, CambiarEstadoPuntoAccesoManejador>();
            servicios.AddScoped<IManejadorPeticion<ObtenerPuntoAccesoPorIdConsulta, Resultado<PuntoAccesoDto>>, ObtenerPuntoAccesoPorIdManejador>();
            servicios.AddScoped<IManejadorPeticion<ListarPuntosAccesoConsulta, Resultado<RespuestaPaginada<PuntoAccesoDto>>>, ListarPuntosAccesoManejador>();

            servicios.AddScoped<IManejadorPeticion<RegistrarMovimientoComando, Resultado<ResultadoMovimientoDto>>, RegistrarMovimientoManejador>();
            servicios.AddScoped<IManejadorPeticion<ConsultarHistorialAccesosConsulta, Resultado<RespuestaPaginada<RegistroAccesoDto>>>, ConsultarHistorialAccesosManejador>();

            return servicios;
        }
    }
}
