using ControlAcceso.Aplicacion.CasosDeUso.Comun;
using ControlAcceso.Aplicacion.Contratos.Persistencia;
using ControlAcceso.Aplicacion.Contratos.Repositorios;
using ControlAcceso.Aplicacion.Excepciones;
using ControlAcceso.Aplicacion.Utilidades.Mediador;
using ControlAcceso.Aplicacion.Utilidades.Resultados;
using ControlAcceso.Dominio.Entidades.Empleados;
using ControlAcceso.Dominio.Entidades.Empleados.ObjetosDeValor;
using ControlAcceso.Dominio.Excepciones;
using FluentValidation;

namespace ControlAcceso.Aplicacion.CasosDeUso.Empleados.Comandos
{
    public sealed record ActualizarEmpleadoComando(Guid Id,
                                                   string Nombres,
                                                   string Apellidos,
                                                   string CorreoElectronico,
                                                   string Cargo,
                                                   Guid AreaId) : IPeticion<Resultado>;

    public sealed class ActualizarEmpleadoValidador : AbstractValidator<ActualizarEmpleadoComando>
    {
        public ActualizarEmpleadoValidador()
        {
            RuleFor(comando => comando.Id)
                .NotEmpty().WithMessage("El identificador del empleado es requerido.");

            RuleFor(comando => comando.Nombres)
                .NotEmpty().WithMessage("Los nombres del empleado son requeridos.")
                .Length(Empleado.LongitudMinimaNombres, Empleado.LongitudMaximaNombres)
                .WithMessage($"Los nombres del empleado deben tener entre {Empleado.LongitudMinimaNombres} y {Empleado.LongitudMaximaNombres} caracteres.");

            RuleFor(comando => comando.Apellidos)
                .NotEmpty().WithMessage("Los apellidos del empleado son requeridos.")
                .Length(Empleado.LongitudMinimaNombres, Empleado.LongitudMaximaNombres)
                .WithMessage($"Los apellidos del empleado deben tener entre {Empleado.LongitudMinimaNombres} y {Empleado.LongitudMaximaNombres} caracteres.");

            RuleFor(comando => comando.CorreoElectronico)
                .NotEmpty().WithMessage("El correo electrónico es requerido.")
                .MaximumLength(CorreoElectronico.LongitudMaxima)
                .WithMessage($"El correo electrónico debe tener máximo {CorreoElectronico.LongitudMaxima} caracteres.")
                .EmailAddress().WithMessage("El correo electrónico no tiene un formato válido.");

            RuleFor(comando => comando.Cargo)
                .NotEmpty().WithMessage("El cargo del empleado es requerido.")
                .Length(Empleado.LongitudMinimaCargo, Empleado.LongitudMaximaCargo)
                .WithMessage($"El cargo del empleado debe tener entre {Empleado.LongitudMinimaCargo} y {Empleado.LongitudMaximaCargo} caracteres.");

            RuleFor(comando => comando.AreaId)
                .NotEmpty().WithMessage("El área del empleado es requerida.");
        }
    }

    public sealed class ActualizarEmpleadoManejador : ManejadorBase, IManejadorPeticion<ActualizarEmpleadoComando, Resultado>
    {
        private readonly IRepositorioEmpleados _repositorioEmpleados;
        private readonly IRepositorioAreas _repositorioAreas;
        private readonly IUnidadDeTrabajo _unidadDeTrabajo;

        public ActualizarEmpleadoManejador(IRepositorioEmpleados repositorioEmpleados,
                                           IRepositorioAreas repositorioAreas,
                                           IUnidadDeTrabajo unidadDeTrabajo)
        {
            _repositorioEmpleados = repositorioEmpleados;
            _repositorioAreas = repositorioAreas;
            _unidadDeTrabajo = unidadDeTrabajo;
        }

        public Task<Resultado> Manejar(ActualizarEmpleadoComando peticion, CancellationToken cancelacion = default)
        {
            return Ejecutar(async () =>
            {
                Empleado empleado = await _repositorioEmpleados.ObtenerPorIdAsync(peticion.Id, cancelacion)
                    ?? throw new ExcepcionNoEncontrado($"No existe un empleado con el identificador {peticion.Id}.");

                CorreoElectronico correoElectronico = new(peticion.CorreoElectronico);

                if (!await _repositorioAreas.ExistePorIdAsync(peticion.AreaId, cancelacion))
                {
                    throw new ExcepcionNoEncontrado($"No existe un área con el identificador {peticion.AreaId}.");
                }

                if (await _repositorioEmpleados.ExistePorCorreoElectronicoAsync(correoElectronico, empleado.Id, cancelacion))
                {
                    throw new ExcepcionReglaNegocio($"Ya existe otro empleado con el correo electrónico {correoElectronico.Valor}.");
                }

                empleado.Actualizar(peticion.Nombres, peticion.Apellidos, correoElectronico, peticion.Cargo, peticion.AreaId);

                _repositorioEmpleados.Actualizar(empleado);
                await _unidadDeTrabajo.ConfirmarAsync(cancelacion);
            }, "Empleado actualizado con éxito");
        }
    }
}
