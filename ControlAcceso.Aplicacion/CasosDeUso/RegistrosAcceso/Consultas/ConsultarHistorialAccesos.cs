using ControlAcceso.Aplicacion.CasosDeUso.Comun;
using ControlAcceso.Aplicacion.CasosDeUso.RegistrosAcceso.Dtos;
using ControlAcceso.Aplicacion.Contratos.Repositorios;
using ControlAcceso.Aplicacion.Utilidades.Mediador;
using ControlAcceso.Aplicacion.Utilidades.Paginacion;
using ControlAcceso.Aplicacion.Utilidades.Resultados;
using ControlAcceso.Dominio.Entidades.RegistrosAcceso;
using ControlAcceso.Dominio.Entidades.RegistrosAcceso.ObjetosDeValor;
using FluentValidation;

namespace ControlAcceso.Aplicacion.CasosDeUso.RegistrosAcceso.Consultas
{
    public sealed record ConsultarHistorialAccesosConsulta(int NumeroPagina,
                                                           int TamanoPagina,
                                                           Guid? EmpleadoId,
                                                           DateTime? Desde,
                                                           DateTime? Hasta,
                                                           Guid? PuntoAccesoId,
                                                           ResultadoAcceso? ResultadoMovimiento)
        : IPeticion<Resultado<RespuestaPaginada<RegistroAccesoDto>>>;

    public sealed class ConsultarHistorialAccesosValidador : AbstractValidator<ConsultarHistorialAccesosConsulta>
    {
        public ConsultarHistorialAccesosValidador()
        {
            RuleFor(consulta => consulta.NumeroPagina)
                .GreaterThan(0).WithMessage("El número de página debe ser mayor a 0.");

            RuleFor(consulta => consulta.TamanoPagina)
                .InclusiveBetween(1, RespuestaPaginada<RegistroAccesoDto>.TamanoPaginaMaximo)
                .WithMessage($"El tamaño de página debe estar entre 1 y {RespuestaPaginada<RegistroAccesoDto>.TamanoPaginaMaximo}.");

            RuleFor(consulta => consulta.Hasta)
                .Must((consulta, hasta) => consulta.Desde is null || hasta is null || consulta.Desde <= hasta)
                .WithMessage("La fecha inicial debe ser anterior o igual a la fecha final.");
        }
    }

    public sealed class ConsultarHistorialAccesosManejador : ManejadorBase, IManejadorPeticion<ConsultarHistorialAccesosConsulta, Resultado<RespuestaPaginada<RegistroAccesoDto>>>
    {
        private readonly IRepositorioRegistrosAcceso _repositorioRegistrosAcceso;

        public ConsultarHistorialAccesosManejador(IRepositorioRegistrosAcceso repositorioRegistrosAcceso)
        {
            _repositorioRegistrosAcceso = repositorioRegistrosAcceso;
        }

        public Task<Resultado<RespuestaPaginada<RegistroAccesoDto>>> Manejar(ConsultarHistorialAccesosConsulta peticion, CancellationToken cancelacion = default)
        {
            return Ejecutar(async () =>
            {
                RespuestaPaginada<RegistroAcceso> pagina = await _repositorioRegistrosAcceso.ObtenerPaginadoAsync(peticion.NumeroPagina,
                                                                                                                 peticion.TamanoPagina,
                                                                                                                 peticion.EmpleadoId,
                                                                                                                 peticion.Desde,
                                                                                                                 peticion.Hasta,
                                                                                                                 peticion.PuntoAccesoId,
                                                                                                                 peticion.ResultadoMovimiento,
                                                                                                                 cancelacion);

                List<RegistroAccesoDto> elementos = pagina.Elementos.Select(RegistroAccesoDto.Desde).ToList();

                return RespuestaPaginada<RegistroAccesoDto>.Crear(elementos, pagina.TotalRegistros, pagina.NumeroPagina, pagina.TamanoPagina);
            });
        }
    }
}
