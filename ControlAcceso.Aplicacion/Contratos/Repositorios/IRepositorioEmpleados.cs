using ControlAcceso.Aplicacion.Utilidades.Paginacion;
using ControlAcceso.Dominio.Entidades.Empleados;
using ControlAcceso.Dominio.Entidades.Empleados.ObjetosDeValor;

namespace ControlAcceso.Aplicacion.Contratos.Repositorios
{
    public interface IRepositorioEmpleados : IRepositorio<Empleado>
    {
        Task<Empleado?> ObtenerPorIdConAreaAsync(Guid id, CancellationToken cancelacion = default);

        Task<RespuestaPaginada<Empleado>> ObtenerPaginadoAsync(int numeroPagina,
                                                               int tamanoPagina,
                                                               Guid? areaId,
                                                               EstadoLaboral? estado,
                                                               CancellationToken cancelacion = default);

        Task<bool> ExistePorNumeroDocumentoAsync(NumeroDocumento numeroDocumento, CancellationToken cancelacion = default);

        Task<bool> ExistePorCorreoElectronicoAsync(CorreoElectronico correoElectronico,
                                                   Guid? excluirEmpleadoId = null,
                                                   CancellationToken cancelacion = default);
    }
}
