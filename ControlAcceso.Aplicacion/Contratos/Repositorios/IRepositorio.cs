namespace ControlAcceso.Aplicacion.Contratos.Repositorios
{
    public interface IRepositorio<TEntidad> where TEntidad : class
    {
        Task<TEntidad?> ObtenerPorIdAsync(Guid id, CancellationToken cancelacion = default);
        Task AgregarAsync(TEntidad entidad, CancellationToken cancelacion = default);
        void Actualizar(TEntidad entidad);
    }
}
