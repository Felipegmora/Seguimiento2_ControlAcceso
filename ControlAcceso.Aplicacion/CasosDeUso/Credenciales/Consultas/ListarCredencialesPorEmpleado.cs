using ControlAcceso.Aplicacion.CasosDeUso.Comun;
using ControlAcceso.Aplicacion.CasosDeUso.Credenciales.Dtos;
using ControlAcceso.Aplicacion.Contratos.Repositorios;
using ControlAcceso.Aplicacion.Excepciones;
using ControlAcceso.Aplicacion.Utilidades.Mediador;
using ControlAcceso.Aplicacion.Utilidades.Resultados;
using ControlAcceso.Dominio.Entidades.Credenciales;
using FluentValidation;

namespace ControlAcceso.Aplicacion.CasosDeUso.Credenciales.Consultas
{
    public sealed record ListarCredencialesPorEmpleadoConsulta(Guid EmpleadoId) : IPeticion<Resultado<List<CredencialDto>>>;

    public sealed class ListarCredencialesPorEmpleadoValidador : AbstractValidator<ListarCredencialesPorEmpleadoConsulta>
    {
        public ListarCredencialesPorEmpleadoValidador()
        {
            RuleFor(consulta => consulta.EmpleadoId)
                .NotEmpty().WithMessage("El identificador del empleado es requerido.");
        }
    }

    public sealed class ListarCredencialesPorEmpleadoManejador : ManejadorBase, IManejadorPeticion<ListarCredencialesPorEmpleadoConsulta, Resultado<List<CredencialDto>>>
    {
        private readonly IRepositorioCredenciales _repositorioCredenciales;
        private readonly IRepositorioEmpleados _repositorioEmpleados;

        public ListarCredencialesPorEmpleadoManejador(IRepositorioCredenciales repositorioCredenciales,
                                                      IRepositorioEmpleados repositorioEmpleados)
        {
            _repositorioCredenciales = repositorioCredenciales;
            _repositorioEmpleados = repositorioEmpleados;
        }

        public Task<Resultado<List<CredencialDto>>> Manejar(ListarCredencialesPorEmpleadoConsulta peticion, CancellationToken cancelacion = default)
        {
            return Ejecutar(async () =>
            {
                if (await _repositorioEmpleados.ObtenerPorIdAsync(peticion.EmpleadoId, cancelacion) is null)
                {
                    throw new ExcepcionNoEncontrado($"No existe un empleado con el identificador {peticion.EmpleadoId}.");
                }

                List<Credencial> credenciales = await _repositorioCredenciales.ObtenerPorEmpleadoAsync(peticion.EmpleadoId, cancelacion);

                return credenciales.Select(CredencialDto.Desde).ToList();
            });
        }
    }
}
