using ControlAcceso.Aplicacion.Utilidades.Paginacion;
using ControlAcceso.Dominio.Entidades.PuntosAcceso;
using ControlAcceso.Dominio.Entidades.PuntosAcceso.ObjetosDeValor;

namespace ControlAcceso.Aplicacion.Contratos.Repositorios
{
    public interface IRepositorioPuntosAcceso : IRepositorio<PuntoAcceso>
    {
        Task<RespuestaPaginada<PuntoAcceso>> ObtenerPaginadoAsync(int numeroPagina,
                                                                  int tamanoPagina,
                                                                  bool? habilitado,
                                                                  TipoPuntoAcceso? tipo,
                                                                  CancellationToken cancelacion = default);

        Task<bool> ExistePorNombreAsync(string nombre, CancellationToken cancelacion = default);
    }
}
