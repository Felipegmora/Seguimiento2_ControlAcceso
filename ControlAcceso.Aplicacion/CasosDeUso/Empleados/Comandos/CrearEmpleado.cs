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
    public sealed record CrearEmpleadoComando(string NumeroDocumento,
                                              string Nombres,
                                              string Apellidos,
                                              string CorreoElectronico,
                                              string Cargo,
                                              Guid AreaId) : IPeticion<Resultado<Guid>>;

    public sealed class CrearEmpleadoValidador : AbstractValidator<CrearEmpleadoComando>
    {
        public CrearEmpleadoValidador()
        {
            RuleFor(comando => comando.NumeroDocumento)
                .NotEmpty().WithMessage("El número de identificación es requerido.")
                .Length(NumeroDocumento.LongitudMinima, NumeroDocumento.LongitudMaxima)
                .WithMessage($"El número de identificación debe tener entre {NumeroDocumento.LongitudMinima} y {NumeroDocumento.LongitudMaxima} caracteres.")
                .Matches("^[A-Za-z0-9]+$").WithMessage("El número de identificación solo puede contener letras y números.");

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

    public sealed class CrearEmpleadoManejador : ManejadorBase, IManejadorPeticion<CrearEmpleadoComando, Resultado<Guid>>
    {
        private readonly IRepositorioEmpleados _repositorioEmpleados;
        private readonly IRepositorioAreas _repositorioAreas;
        private readonly IUnidadDeTrabajo _unidadDeTrabajo;

        public CrearEmpleadoManejador(IRepositorioEmpleados repositorioEmpleados,
                                      IRepositorioAreas repositorioAreas,
                                      IUnidadDeTrabajo unidadDeTrabajo)
        {
            _repositorioEmpleados = repositorioEmpleados;
            _repositorioAreas = repositorioAreas;
            _unidadDeTrabajo = unidadDeTrabajo;
        }

        public Task<Resultado<Guid>> Manejar(CrearEmpleadoComando peticion, CancellationToken cancelacion = default)
        {
            return Ejecutar(async () =>
            {
                NumeroDocumento numeroDocumento = new(peticion.NumeroDocumento);
                CorreoElectronico correoElectronico = new(peticion.CorreoElectronico);

                if (!await _repositorioAreas.ExistePorIdAsync(peticion.AreaId, cancelacion))
                {
                    throw new ExcepcionNoEncontrado($"No existe un área con el identificador {peticion.AreaId}.");
                }

                if (await _repositorioEmpleados.ExistePorNumeroDocumentoAsync(numeroDocumento, cancelacion))
                {
                    throw new ExcepcionReglaNegocio($"Ya existe un empleado con el número de identificación {numeroDocumento.Valor}.");
                }

                if (await _repositorioEmpleados.ExistePorCorreoElectronicoAsync(correoElectronico, null, cancelacion))
                {
                    throw new ExcepcionReglaNegocio($"Ya existe un empleado con el correo electrónico {correoElectronico.Valor}.");
                }

                Empleado empleado = new(numeroDocumento,
                                        peticion.Nombres,
                                        peticion.Apellidos,
                                        correoElectronico,
                                        peticion.Cargo,
                                        peticion.AreaId);

                await _repositorioEmpleados.AgregarAsync(empleado, cancelacion);
                await _unidadDeTrabajo.ConfirmarAsync(cancelacion);

                return empleado.Id;
            }, "Empleado registrado con éxito");
        }
    }
}
