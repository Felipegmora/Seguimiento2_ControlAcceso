using ControlAcceso.Aplicacion.Contratos.Repositorios;
using ControlAcceso.Aplicacion.Utilidades.Paginacion;
using ControlAcceso.Dominio.Entidades.RegistrosAcceso;
using ControlAcceso.Dominio.Entidades.RegistrosAcceso.ObjetosDeValor;
using ControlAcceso.Persistencia.Extensiones;
using Microsoft.EntityFrameworkCore;

namespace ControlAcceso.Persistencia.Repositorios
{
    public sealed class RepositorioRegistrosAcceso : Repositorio<RegistroAcceso>, IRepositorioRegistrosAcceso
    {
        public RepositorioRegistrosAcceso(ContextoDatos contexto) : base(contexto)
        {
        }

        public async Task<RespuestaPaginada<RegistroAcceso>> ObtenerPaginadoAsync(int numeroPagina,
                                                                                  int tamanoPagina,
                                                                                  Guid? empleadoId,
                                                                                  DateTime? desde,
                                                                                  DateTime? hasta,
                                                                                  Guid? puntoAccesoId,
                                                                                  ResultadoAcceso? resultado,
                                                                                  CancellationToken cancelacion = default)
        {
            IQueryable<RegistroAcceso> consulta = _contexto.RegistrosAcceso.AsNoTracking()
                                                                           .Include(registro => registro.Empleado)
                                                                           .Include(registro => registro.Credencial)
                                                                           .Include(registro => registro.PuntoAcceso);

            if (empleadoId.HasValue)
            {
                consulta = consulta.Where(registro => registro.EmpleadoId == empleadoId.Value);
            }

            if (desde.HasValue)
            {
                DateTime fechaInicial = desde.Value.ToUniversalTime();

                consulta = consulta.Where(registro => registro.FechaMovimiento >= fechaInicial);
            }

            if (hasta.HasValue)
            {
                DateTime fechaFinal = hasta.Value.ToUniversalTime();

                consulta = consulta.Where(registro => registro.FechaMovimiento <= fechaFinal);
            }

            if (puntoAccesoId.HasValue)
            {
                consulta = consulta.Where(registro => registro.PuntoAccesoId == puntoAccesoId.Value);
            }

            if (resultado.HasValue)
            {
                consulta = consulta.Where(registro => registro.Resultado == resultado.Value);
            }

            consulta = consulta.OrderByDescending(registro => registro.FechaMovimiento)
                               .ThenByDescending(registro => registro.Id);

            return await consulta.APaginaAsync(numeroPagina, tamanoPagina, cancelacion);
        }

        public async Task<RegistroAcceso?> ObtenerUltimoAutorizadoPorEmpleadoAsync(Guid empleadoId, CancellationToken cancelacion = default)
        {
            return await _contexto.RegistrosAcceso.AsNoTracking()
                                                  .Where(registro => registro.EmpleadoId == empleadoId
                                                                     && registro.Resultado == ResultadoAcceso.Autorizado)
                                                  .OrderByDescending(registro => registro.FechaMovimiento)
                                                  .ThenByDescending(registro => registro.Id)
                                                  .FirstOrDefaultAsync(cancelacion);
        }
    }
}
