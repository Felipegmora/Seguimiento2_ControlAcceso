using ControlAcceso.Aplicacion.CasosDeUso.Comun;
using ControlAcceso.Aplicacion.CasosDeUso.PuntosAcceso.Dtos;
using ControlAcceso.Aplicacion.Contratos.Repositorios;
using ControlAcceso.Aplicacion.Excepciones;
using ControlAcceso.Aplicacion.Utilidades.Mediador;
using ControlAcceso.Aplicacion.Utilidades.Resultados;
using ControlAcceso.Dominio.Entidades.PuntosAcceso;
using FluentValidation;

namespace ControlAcceso.Aplicacion.CasosDeUso.PuntosAcceso.Consultas
{
    public sealed record ObtenerPuntoAccesoPorIdConsulta(Guid Id) : IPeticion<Resultado<PuntoAccesoDto>>;

    public sealed class ObtenerPuntoAccesoPorIdValidador : AbstractValidator<ObtenerPuntoAccesoPorIdConsulta>
    {
        public ObtenerPuntoAccesoPorIdValidador()
        {
            RuleFor(consulta => consulta.Id)
                .NotEmpty().WithMessage("El identificador del punto de acceso es requerido.");
        }
    }

    public sealed class ObtenerPuntoAccesoPorIdManejador : ManejadorBase, IManejadorPeticion<ObtenerPuntoAccesoPorIdConsulta, Resultado<PuntoAccesoDto>>
    {
        private readonly IRepositorioPuntosAcceso _repositorioPuntosAcceso;

        public ObtenerPuntoAccesoPorIdManejador(IRepositorioPuntosAcceso repositorioPuntosAcceso)
        {
            _repositorioPuntosAcceso = repositorioPuntosAcceso;
        }

        public Task<Resultado<PuntoAccesoDto>> Manejar(ObtenerPuntoAccesoPorIdConsulta peticion, CancellationToken cancelacion = default)
        {
            return Ejecutar(async () =>
            {
                PuntoAcceso puntoAcceso = await _repositorioPuntosAcceso.ObtenerPorIdAsync(peticion.Id, cancelacion)
                    ?? throw new ExcepcionNoEncontrado($"No existe un punto de acceso con el identificador {peticion.Id}.");

                return PuntoAccesoDto.Desde(puntoAcceso);
            });
        }
    }
}
