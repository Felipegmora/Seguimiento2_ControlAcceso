using ControlAcceso.Aplicacion.Contratos.Repositorios;
using ControlAcceso.Dominio.Entidades.Credenciales;
using Microsoft.EntityFrameworkCore;

namespace ControlAcceso.Persistencia.Repositorios
{
    public sealed class RepositorioCredenciales : Repositorio<Credencial>, IRepositorioCredenciales
    {
        public RepositorioCredenciales(ContextoDatos contexto) : base(contexto)
        {
        }

        public async Task<Credencial?> ObtenerPorCodigoAsync(string codigo, CancellationToken cancelacion = default)
        {
            string codigoNormalizado = Credencial.NormalizarCodigo(codigo);

            return await _contexto.Credenciales.AsNoTracking()
                                               .FirstOrDefaultAsync(credencial => credencial.Codigo == codigoNormalizado, cancelacion);
        }

        public async Task<Credencial?> ObtenerPorCodigoConEmpleadoAsync(string codigo, CancellationToken cancelacion = default)
        {
            string codigoNormalizado = Credencial.NormalizarCodigo(codigo);

            return await _contexto.Credenciales.AsNoTracking()
                                               .Include(credencial => credencial.Empleado)
                                               .FirstOrDefaultAsync(credencial => credencial.Codigo == codigoNormalizado, cancelacion);
        }

        public async Task<List<Credencial>> ObtenerPorEmpleadoAsync(Guid empleadoId, CancellationToken cancelacion = default)
        {
            return await _contexto.Credenciales.AsNoTracking()
                                               .Include(credencial => credencial.Empleado)
                                               .Where(credencial => credencial.EmpleadoId == empleadoId)
                                               .OrderByDescending(credencial => credencial.FechaCreacion)
                                               .ThenBy(credencial => credencial.Id)
                                               .ToListAsync(cancelacion);
        }

        public async Task<bool> ExistePorCodigoAsync(string codigo, CancellationToken cancelacion = default)
        {
            string codigoNormalizado = Credencial.NormalizarCodigo(codigo);

            return await _contexto.Credenciales.AnyAsync(credencial => credencial.Codigo == codigoNormalizado, cancelacion);
        }
    }
}
