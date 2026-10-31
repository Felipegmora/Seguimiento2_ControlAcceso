using ControlAcceso.Dominio.Entidades.Credenciales;

namespace ControlAcceso.Aplicacion.Contratos.Repositorios
{
    public interface IRepositorioCredenciales : IRepositorio<Credencial>
    {
        Task<Credencial?> ObtenerPorCodigoAsync(string codigo, CancellationToken cancelacion = default);

        Task<Credencial?> ObtenerPorCodigoConEmpleadoAsync(string codigo, CancellationToken cancelacion = default);

        Task<List<Credencial>> ObtenerPorEmpleadoAsync(Guid empleadoId, CancellationToken cancelacion = default);

        Task<bool> ExistePorCodigoAsync(string codigo, CancellationToken cancelacion = default);
    }
}
