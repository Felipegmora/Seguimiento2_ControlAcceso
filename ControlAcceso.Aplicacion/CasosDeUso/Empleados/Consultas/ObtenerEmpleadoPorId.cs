using ControlAcceso.Aplicacion.CasosDeUso.Comun;
using ControlAcceso.Aplicacion.CasosDeUso.Empleados.Dtos;
using ControlAcceso.Aplicacion.Contratos.Repositorios;
using ControlAcceso.Aplicacion.Excepciones;
using ControlAcceso.Aplicacion.Utilidades.Mediador;
using ControlAcceso.Aplicacion.Utilidades.Resultados;
using ControlAcceso.Dominio.Entidades.Empleados;
using FluentValidation;

namespace ControlAcceso.Aplicacion.CasosDeUso.Empleados.Consultas
{
    public sealed record ObtenerEmpleadoPorIdConsulta(Guid Id) : IPeticion<Resultado<EmpleadoDto>>;

    public sealed class ObtenerEmpleadoPorIdValidador : AbstractValidator<ObtenerEmpleadoPorIdConsulta>
    {
        public ObtenerEmpleadoPorIdValidador()
        {
            RuleFor(consulta => consulta.Id)
                .NotEmpty().WithMessage("El identificador del empleado es requerido.");
        }
    }

    public sealed class ObtenerEmpleadoPorIdManejador : ManejadorBase, IManejadorPeticion<ObtenerEmpleadoPorIdConsulta, Resultado<EmpleadoDto>>
    {
        private readonly IRepositorioEmpleados _repositorioEmpleados;

        public ObtenerEmpleadoPorIdManejador(IRepositorioEmpleados repositorioEmpleados)
        {
            _repositorioEmpleados = repositorioEmpleados;
        }

        public Task<Resultado<EmpleadoDto>> Manejar(ObtenerEmpleadoPorIdConsulta peticion, CancellationToken cancelacion = default)
        {
            return Ejecutar(async () =>
            {
                Empleado empleado = await _repositorioEmpleados.ObtenerPorIdConAreaAsync(peticion.Id, cancelacion)
                    ?? throw new ExcepcionNoEncontrado($"No existe un empleado con el identificador {peticion.Id}.");

                return EmpleadoDto.Desde(empleado);
            });
        }
    }
}
