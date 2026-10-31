using ControlAcceso.Aplicacion.CasosDeUso.Comun;
using ControlAcceso.Aplicacion.CasosDeUso.Empleados.Dtos;
using ControlAcceso.Aplicacion.Contratos.Repositorios;
using ControlAcceso.Aplicacion.Utilidades.Mediador;
using ControlAcceso.Aplicacion.Utilidades.Paginacion;
using ControlAcceso.Aplicacion.Utilidades.Resultados;
using ControlAcceso.Dominio.Entidades.Empleados;
using ControlAcceso.Dominio.Entidades.Empleados.ObjetosDeValor;
using FluentValidation;

namespace ControlAcceso.Aplicacion.CasosDeUso.Empleados.Consultas
{
    public sealed record ListarEmpleadosConsulta(int NumeroPagina,
                                                 int TamanoPagina,
                                                 Guid? AreaId,
                                                 EstadoLaboral? Estado) : IPeticion<Resultado<RespuestaPaginada<EmpleadoDto>>>;

    public sealed class ListarEmpleadosValidador : AbstractValidator<ListarEmpleadosConsulta>
    {
        public ListarEmpleadosValidador()
        {
            RuleFor(consulta => consulta.NumeroPagina)
                .GreaterThan(0).WithMessage("El número de página debe ser mayor a 0.");

            RuleFor(consulta => consulta.TamanoPagina)
                .InclusiveBetween(1, RespuestaPaginada<EmpleadoDto>.TamanoPaginaMaximo)
                .WithMessage($"El tamaño de página debe estar entre 1 y {RespuestaPaginada<EmpleadoDto>.TamanoPaginaMaximo}.");
        }
    }

    public sealed class ListarEmpleadosManejador : ManejadorBase, IManejadorPeticion<ListarEmpleadosConsulta, Resultado<RespuestaPaginada<EmpleadoDto>>>
    {
        private readonly IRepositorioEmpleados _repositorioEmpleados;

        public ListarEmpleadosManejador(IRepositorioEmpleados repositorioEmpleados)
        {
            _repositorioEmpleados = repositorioEmpleados;
        }

        public Task<Resultado<RespuestaPaginada<EmpleadoDto>>> Manejar(ListarEmpleadosConsulta peticion, CancellationToken cancelacion = default)
        {
            return Ejecutar(async () =>
            {
                RespuestaPaginada<Empleado> pagina = await _repositorioEmpleados.ObtenerPaginadoAsync(peticion.NumeroPagina,
                                                                                                     peticion.TamanoPagina,
                                                                                                     peticion.AreaId,
                                                                                                     peticion.Estado,
                                                                                                     cancelacion);

                List<EmpleadoDto> elementos = pagina.Elementos.Select(EmpleadoDto.Desde).ToList();

                return RespuestaPaginada<EmpleadoDto>.Crear(elementos, pagina.TotalRegistros, pagina.NumeroPagina, pagina.TamanoPagina);
            });
        }
    }
}
