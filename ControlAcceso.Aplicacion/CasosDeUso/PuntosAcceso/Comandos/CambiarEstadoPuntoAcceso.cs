using ControlAcceso.Aplicacion.CasosDeUso.Comun;
using ControlAcceso.Aplicacion.Contratos.Persistencia;
using ControlAcceso.Aplicacion.Contratos.Repositorios;
using ControlAcceso.Aplicacion.Excepciones;
using ControlAcceso.Aplicacion.Utilidades.Mediador;
using ControlAcceso.Aplicacion.Utilidades.Resultados;
using ControlAcceso.Dominio.Entidades.PuntosAcceso;
using FluentValidation;

namespace ControlAcceso.Aplicacion.CasosDeUso.PuntosAcceso.Comandos
{
    public sealed record CambiarEstadoPuntoAccesoComando(Guid Id, bool Habilitado) : IPeticion<Resultado>;

    public sealed class CambiarEstadoPuntoAccesoValidador : AbstractValidator<CambiarEstadoPuntoAccesoComando>
    {
        public CambiarEstadoPuntoAccesoValidador()
        {
            RuleFor(comando => comando.Id)
                .NotEmpty().WithMessage("El identificador del punto de acceso es requerido.");
        }
    }

    public sealed class CambiarEstadoPuntoAccesoManejador : ManejadorBase, IManejadorPeticion<CambiarEstadoPuntoAccesoComando, Resultado>
    {
        private readonly IRepositorioPuntosAcceso _repositorioPuntosAcceso;
        private readonly IUnidadDeTrabajo _unidadDeTrabajo;

        public CambiarEstadoPuntoAccesoManejador(IRepositorioPuntosAcceso repositorioPuntosAcceso, IUnidadDeTrabajo unidadDeTrabajo)
        {
            _repositorioPuntosAcceso = repositorioPuntosAcceso;
            _unidadDeTrabajo = unidadDeTrabajo;
        }

        public Task<Resultado> Manejar(CambiarEstadoPuntoAccesoComando peticion, CancellationToken cancelacion = default)
        {
            return Ejecutar(async () =>
            {
                PuntoAcceso puntoAcceso = await _repositorioPuntosAcceso.ObtenerPorIdAsync(peticion.Id, cancelacion)
                    ?? throw new ExcepcionNoEncontrado($"No existe un punto de acceso con el identificador {peticion.Id}.");

                if (peticion.Habilitado)
                {
                    puntoAcceso.Habilitar();
                }
                else
                {
                    puntoAcceso.Deshabilitar();
                }

                _repositorioPuntosAcceso.Actualizar(puntoAcceso);
                await _unidadDeTrabajo.ConfirmarAsync(cancelacion);
            }, "Estado del punto de acceso actualizado con éxito");
        }
    }
}
