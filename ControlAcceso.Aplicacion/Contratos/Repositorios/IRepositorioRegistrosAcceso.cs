using ControlAcceso.Aplicacion.Utilidades.Paginacion;
using ControlAcceso.Dominio.Entidades.RegistrosAcceso;
using ControlAcceso.Dominio.Entidades.RegistrosAcceso.ObjetosDeValor;

namespace ControlAcceso.Aplicacion.Contratos.Repositorios
{
    public interface IRepositorioRegistrosAcceso : IRepositorio<RegistroAcceso>
    {
        Task<RespuestaPaginada<RegistroAcceso>> ObtenerPaginadoAsync(int numeroPagina,
                                                                     int tamanoPagina,
                                                                     Guid? empleadoId,
                                                                     DateTime? desde,
                                                                     DateTime? hasta,
                                                                     Guid? puntoAccesoId,
                                                                     ResultadoAcceso? resultado,
                                                                     CancellationToken cancelacion = default);

        Task<RegistroAcceso?> ObtenerUltimoAutorizadoPorEmpleadoAsync(Guid empleadoId, CancellationToken cancelacion = default);
    }
}
