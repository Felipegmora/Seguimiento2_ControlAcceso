using ControlAcceso.Aplicacion.CasosDeUso.Comun;
using ControlAcceso.Aplicacion.Contratos.Persistencia;
using ControlAcceso.Aplicacion.Contratos.Repositorios;
using ControlAcceso.Aplicacion.Excepciones;
using ControlAcceso.Aplicacion.Utilidades.Mediador;
using ControlAcceso.Aplicacion.Utilidades.Resultados;
using ControlAcceso.Dominio.Entidades.Credenciales;
using FluentValidation;

namespace ControlAcceso.Aplicacion.CasosDeUso.Credenciales.Comandos
{
    public sealed record BloquearCredencialComando(Guid Id) : IPeticion<Resultado>;

    public sealed class BloquearCredencialValidador : AbstractValidator<BloquearCredencialComando>
    {
        public BloquearCredencialValidador()
        {
            RuleFor(comando => comando.Id)
                .NotEmpty().WithMessage("El identificador de la credencial es requerido.");
        }
    }

    public sealed class BloquearCredencialManejador : ManejadorBase, IManejadorPeticion<BloquearCredencialComando, Resultado>
    {
        private readonly IRepositorioCredenciales _repositorioCredenciales;
        private readonly IUnidadDeTrabajo _unidadDeTrabajo;

        public BloquearCredencialManejador(IRepositorioCredenciales repositorioCredenciales, IUnidadDeTrabajo unidadDeTrabajo)
        {
            _repositorioCredenciales = repositorioCredenciales;
            _unidadDeTrabajo = unidadDeTrabajo;
        }

        public Task<Resultado> Manejar(BloquearCredencialComando peticion, CancellationToken cancelacion = default)
        {
            return Ejecutar(async () =>
            {
                Credencial credencial = await _repositorioCredenciales.ObtenerPorIdAsync(peticion.Id, cancelacion)
                    ?? throw new ExcepcionNoEncontrado($"No existe una credencial con el identificador {peticion.Id}.");

                credencial.Bloquear();

                _repositorioCredenciales.Actualizar(credencial);
                await _unidadDeTrabajo.ConfirmarAsync(cancelacion);
            }, "Credencial bloqueada con éxito");
        }
    }
}
