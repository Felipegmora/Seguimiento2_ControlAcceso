using ControlAcceso.Aplicacion.CasosDeUso.Comun;
using ControlAcceso.Aplicacion.Contratos.Persistencia;
using ControlAcceso.Aplicacion.Contratos.Repositorios;
using ControlAcceso.Aplicacion.Utilidades.Mediador;
using ControlAcceso.Aplicacion.Utilidades.Resultados;
using ControlAcceso.Dominio.Entidades.PuntosAcceso;
using ControlAcceso.Dominio.Entidades.PuntosAcceso.ObjetosDeValor;
using ControlAcceso.Dominio.Excepciones;
using FluentValidation;

namespace ControlAcceso.Aplicacion.CasosDeUso.PuntosAcceso.Comandos
{
    public sealed record RegistrarPuntoAccesoComando(string Nombre,
                                                     TipoPuntoAcceso Tipo,
                                                     string? Descripcion) : IPeticion<Resultado<Guid>>;

    public sealed class RegistrarPuntoAccesoValidador : AbstractValidator<RegistrarPuntoAccesoComando>
    {
        public RegistrarPuntoAccesoValidador()
        {
            RuleFor(comando => comando.Nombre)
                .NotEmpty().WithMessage("El nombre del punto de acceso es requerido.")
                .Length(PuntoAcceso.LongitudMinimaNombre, PuntoAcceso.LongitudMaximaNombre)
                .WithMessage($"El nombre del punto de acceso debe tener entre {PuntoAcceso.LongitudMinimaNombre} y {PuntoAcceso.LongitudMaximaNombre} caracteres.");

            RuleFor(comando => comando.Tipo)
                .IsInEnum().WithMessage("El tipo del punto de acceso no es válido.");

            RuleFor(comando => comando.Descripcion)
                .MaximumLength(PuntoAcceso.LongitudMaximaDescripcion)
                .WithMessage($"La descripción del punto de acceso debe tener máximo {PuntoAcceso.LongitudMaximaDescripcion} caracteres.");
        }
    }

    public sealed class RegistrarPuntoAccesoManejador : ManejadorBase, IManejadorPeticion<RegistrarPuntoAccesoComando, Resultado<Guid>>
    {
        private readonly IRepositorioPuntosAcceso _repositorioPuntosAcceso;
        private readonly IUnidadDeTrabajo _unidadDeTrabajo;

        public RegistrarPuntoAccesoManejador(IRepositorioPuntosAcceso repositorioPuntosAcceso, IUnidadDeTrabajo unidadDeTrabajo)
        {
            _repositorioPuntosAcceso = repositorioPuntosAcceso;
            _unidadDeTrabajo = unidadDeTrabajo;
        }

        public Task<Resultado<Guid>> Manejar(RegistrarPuntoAccesoComando peticion, CancellationToken cancelacion = default)
        {
            return Ejecutar(async () =>
            {
                if (await _repositorioPuntosAcceso.ExistePorNombreAsync(peticion.Nombre, cancelacion))
                {
                    throw new ExcepcionReglaNegocio($"Ya existe un punto de acceso con el nombre {peticion.Nombre.Trim()}.");
                }

                PuntoAcceso puntoAcceso = new(peticion.Nombre, peticion.Tipo, peticion.Descripcion);

                await _repositorioPuntosAcceso.AgregarAsync(puntoAcceso, cancelacion);
                await _unidadDeTrabajo.ConfirmarAsync(cancelacion);

                return puntoAcceso.Id;
            }, "Punto de acceso registrado con éxito");
        }
    }
}
