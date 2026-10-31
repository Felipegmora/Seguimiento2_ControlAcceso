using ControlAcceso.Aplicacion.CasosDeUso.Comun;
using ControlAcceso.Aplicacion.Contratos.Persistencia;
using ControlAcceso.Aplicacion.Contratos.Repositorios;
using ControlAcceso.Aplicacion.Excepciones;
using ControlAcceso.Aplicacion.Utilidades.Mediador;
using ControlAcceso.Aplicacion.Utilidades.Resultados;
using ControlAcceso.Dominio.Entidades.Empleados;
using FluentValidation;

namespace ControlAcceso.Aplicacion.CasosDeUso.Empleados.Comandos
{
    public sealed record DesactivarEmpleadoComando(Guid Id) : IPeticion<Resultado>;

    public sealed class DesactivarEmpleadoValidador : AbstractValidator<DesactivarEmpleadoComando>
    {
        public DesactivarEmpleadoValidador()
        {
            RuleFor(comando => comando.Id)
                .NotEmpty().WithMessage("El identificador del empleado es requerido.");
        }
    }

    public sealed class DesactivarEmpleadoManejador : ManejadorBase, IManejadorPeticion<DesactivarEmpleadoComando, Resultado>
    {
        private readonly IRepositorioEmpleados _repositorioEmpleados;
        private readonly IUnidadDeTrabajo _unidadDeTrabajo;

        public DesactivarEmpleadoManejador(IRepositorioEmpleados repositorioEmpleados, IUnidadDeTrabajo unidadDeTrabajo)
        {
            _repositorioEmpleados = repositorioEmpleados;
            _unidadDeTrabajo = unidadDeTrabajo;
        }

        public Task<Resultado> Manejar(DesactivarEmpleadoComando peticion, CancellationToken cancelacion = default)
        {
            return Ejecutar(async () =>
            {
                Empleado empleado = await _repositorioEmpleados.ObtenerPorIdAsync(peticion.Id, cancelacion)
                    ?? throw new ExcepcionNoEncontrado($"No existe un empleado con el identificador {peticion.Id}.");

                empleado.Desactivar();

                _repositorioEmpleados.Actualizar(empleado);
                await _unidadDeTrabajo.ConfirmarAsync(cancelacion);
            }, "Empleado desactivado con éxito");
        }
    }
}
