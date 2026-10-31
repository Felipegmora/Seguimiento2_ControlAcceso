using ControlAcceso.Aplicacion.Contratos.Repositorios;

namespace ControlAcceso.Persistencia.Repositorios
{
    public class Repositorio<TEntidad> : IRepositorio<TEntidad> where TEntidad : class
    {
        protected readonly ContextoDatos _contexto;

        public Repositorio(ContextoDatos contexto)
        {
            _contexto = contexto;
        }

        public async Task<TEntidad?> ObtenerPorIdAsync(Guid id, CancellationToken cancelacion = default)
        {
            return await _contexto.Set<TEntidad>().FindAsync(new object?[] { id }, cancelacion);
        }

        public async Task AgregarAsync(TEntidad entidad, CancellationToken cancelacion = default)
        {
            await _contexto.Set<TEntidad>().AddAsync(entidad, cancelacion);
        }

        public void Actualizar(TEntidad entidad)
        {
            _contexto.Set<TEntidad>().Update(entidad);
        }
    }
}
