using ControlAcceso.Dominio.Entidades.Areas;

namespace ControlAcceso.Aplicacion.Contratos.Repositorios
{
    public interface IRepositorioAreas : IRepositorio<Area>
    {
        Task<List<Area>> ObtenerTodasAsync(CancellationToken cancelacion = default);

        Task<bool> ExistePorIdAsync(Guid id, CancellationToken cancelacion = default);
    }
}
