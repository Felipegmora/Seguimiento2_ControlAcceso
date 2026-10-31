using ControlAcceso.Aplicacion.Contratos.Repositorios;
using ControlAcceso.Dominio.Entidades.Areas;
using Microsoft.EntityFrameworkCore;

namespace ControlAcceso.Persistencia.Repositorios
{
    public sealed class RepositorioAreas : Repositorio<Area>, IRepositorioAreas
    {
        public RepositorioAreas(ContextoDatos contexto) : base(contexto)
        {
        }

        public async Task<List<Area>> ObtenerTodasAsync(CancellationToken cancelacion = default)
        {
            return await _contexto.Areas.AsNoTracking()
                                        .OrderBy(area => area.Nombre)
                                        .ToListAsync(cancelacion);
        }

        public async Task<bool> ExistePorIdAsync(Guid id, CancellationToken cancelacion = default)
        {
            return await _contexto.Areas.AnyAsync(area => area.Id == id, cancelacion);
        }
    }
}
