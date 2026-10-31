using ControlAcceso.Aplicacion.CasosDeUso.Areas.Dtos;
using ControlAcceso.Aplicacion.CasosDeUso.Comun;
using ControlAcceso.Aplicacion.Contratos.Repositorios;
using ControlAcceso.Aplicacion.Utilidades.Mediador;
using ControlAcceso.Aplicacion.Utilidades.Resultados;
using ControlAcceso.Dominio.Entidades.Areas;

namespace ControlAcceso.Aplicacion.CasosDeUso.Areas.Consultas
{
    public sealed record ListarAreasConsulta() : IPeticion<Resultado<List<AreaDto>>>;

    public sealed class ListarAreasManejador : ManejadorBase, IManejadorPeticion<ListarAreasConsulta, Resultado<List<AreaDto>>>
    {
        private readonly IRepositorioAreas _repositorioAreas;

        public ListarAreasManejador(IRepositorioAreas repositorioAreas)
        {
            _repositorioAreas = repositorioAreas;
        }

        public Task<Resultado<List<AreaDto>>> Manejar(ListarAreasConsulta peticion, CancellationToken cancelacion = default)
        {
            return Ejecutar(async () =>
            {
                List<Area> areas = await _repositorioAreas.ObtenerTodasAsync(cancelacion);

                return areas.Select(AreaDto.Desde).ToList();
            });
        }
    }
}
