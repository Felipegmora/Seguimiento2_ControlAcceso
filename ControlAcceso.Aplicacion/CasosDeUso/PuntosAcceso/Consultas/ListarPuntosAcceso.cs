using ControlAcceso.Aplicacion.CasosDeUso.Comun;
using ControlAcceso.Aplicacion.CasosDeUso.PuntosAcceso.Dtos;
using ControlAcceso.Aplicacion.Contratos.Repositorios;
using ControlAcceso.Aplicacion.Utilidades.Mediador;
using ControlAcceso.Aplicacion.Utilidades.Paginacion;
using ControlAcceso.Aplicacion.Utilidades.Resultados;
using ControlAcceso.Dominio.Entidades.PuntosAcceso;
using ControlAcceso.Dominio.Entidades.PuntosAcceso.ObjetosDeValor;
using FluentValidation;

namespace ControlAcceso.Aplicacion.CasosDeUso.PuntosAcceso.Consultas
{
    public sealed record ListarPuntosAccesoConsulta(int NumeroPagina,
                                                    int TamanoPagina,
                                                    bool? Habilitado,
                                                    TipoPuntoAcceso? Tipo) : IPeticion<Resultado<RespuestaPaginada<PuntoAccesoDto>>>;

    public sealed class ListarPuntosAccesoValidador : AbstractValidator<ListarPuntosAccesoConsulta>
    {
        public ListarPuntosAccesoValidador()
        {
            RuleFor(consulta => consulta.NumeroPagina)
                .GreaterThan(0).WithMessage("El número de página debe ser mayor a 0.");

            RuleFor(consulta => consulta.TamanoPagina)
                .InclusiveBetween(1, RespuestaPaginada<PuntoAccesoDto>.TamanoPaginaMaximo)
                .WithMessage($"El tamaño de página debe estar entre 1 y {RespuestaPaginada<PuntoAccesoDto>.TamanoPaginaMaximo}.");
        }
    }

    public sealed class ListarPuntosAccesoManejador : ManejadorBase, IManejadorPeticion<ListarPuntosAccesoConsulta, Resultado<RespuestaPaginada<PuntoAccesoDto>>>
    {
        private readonly IRepositorioPuntosAcceso _repositorioPuntosAcceso;

        public ListarPuntosAccesoManejador(IRepositorioPuntosAcceso repositorioPuntosAcceso)
        {
            _repositorioPuntosAcceso = repositorioPuntosAcceso;
        }

        public Task<Resultado<RespuestaPaginada<PuntoAccesoDto>>> Manejar(ListarPuntosAccesoConsulta peticion, CancellationToken cancelacion = default)
        {
            return Ejecutar(async () =>
            {
                RespuestaPaginada<PuntoAcceso> pagina = await _repositorioPuntosAcceso.ObtenerPaginadoAsync(peticion.NumeroPagina,
                                                                                                           peticion.TamanoPagina,
                                                                                                           peticion.Habilitado,
                                                                                                           peticion.Tipo,
                                                                                                           cancelacion);

                List<PuntoAccesoDto> elementos = pagina.Elementos.Select(PuntoAccesoDto.Desde).ToList();

                return RespuestaPaginada<PuntoAccesoDto>.Crear(elementos, pagina.TotalRegistros, pagina.NumeroPagina, pagina.TamanoPagina);
            });
        }
    }
}
