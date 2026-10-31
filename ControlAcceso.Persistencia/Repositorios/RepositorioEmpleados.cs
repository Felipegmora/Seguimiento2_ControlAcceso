using ControlAcceso.Aplicacion.Contratos.Repositorios;
using ControlAcceso.Aplicacion.Utilidades.Paginacion;
using ControlAcceso.Dominio.Entidades.Empleados;
using ControlAcceso.Dominio.Entidades.Empleados.ObjetosDeValor;
using ControlAcceso.Persistencia.Extensiones;
using Microsoft.EntityFrameworkCore;

namespace ControlAcceso.Persistencia.Repositorios
{
    public sealed class RepositorioEmpleados : Repositorio<Empleado>, IRepositorioEmpleados
    {
        public RepositorioEmpleados(ContextoDatos contexto) : base(contexto)
        {
        }

        public async Task<Empleado?> ObtenerPorIdConAreaAsync(Guid id, CancellationToken cancelacion = default)
        {
            return await _contexto.Empleados.AsNoTracking()
                                            .Include(empleado => empleado.Area)
                                            .FirstOrDefaultAsync(empleado => empleado.Id == id, cancelacion);
        }

        public async Task<RespuestaPaginada<Empleado>> ObtenerPaginadoAsync(int numeroPagina,
                                                                            int tamanoPagina,
                                                                            Guid? areaId,
                                                                            EstadoLaboral? estado,
                                                                            CancellationToken cancelacion = default)
        {
            IQueryable<Empleado> consulta = _contexto.Empleados.AsNoTracking()
                                                               .Include(empleado => empleado.Area);

            if (areaId.HasValue)
            {
                consulta = consulta.Where(empleado => empleado.AreaId == areaId.Value);
            }

            if (estado.HasValue)
            {
                consulta = consulta.Where(empleado => empleado.Estado == estado.Value);
            }

            consulta = consulta.OrderByDescending(empleado => empleado.FechaCreacion)
                               .ThenBy(empleado => empleado.Id);

            return await consulta.APaginaAsync(numeroPagina, tamanoPagina, cancelacion);
        }

        public async Task<bool> ExistePorNumeroDocumentoAsync(NumeroDocumento numeroDocumento, CancellationToken cancelacion = default)
        {
            return await _contexto.Empleados.AnyAsync(empleado => empleado.NumeroDocumento == numeroDocumento, cancelacion);
        }

        public async Task<bool> ExistePorCorreoElectronicoAsync(CorreoElectronico correoElectronico,
                                                                Guid? excluirEmpleadoId = null,
                                                                CancellationToken cancelacion = default)
        {
            IQueryable<Empleado> consulta = _contexto.Empleados.Where(empleado => empleado.CorreoElectronico == correoElectronico);

            if (excluirEmpleadoId.HasValue)
            {
                consulta = consulta.Where(empleado => empleado.Id != excluirEmpleadoId.Value);
            }

            return await consulta.AnyAsync(cancelacion);
        }
    }
}
