using ControlAcceso.Aplicacion.Contratos.Repositorios;
using ControlAcceso.Aplicacion.Utilidades.Paginacion;
using ControlAcceso.Dominio.Entidades.PuntosAcceso;
using ControlAcceso.Dominio.Entidades.PuntosAcceso.ObjetosDeValor;
using ControlAcceso.Persistencia.Extensiones;
using Microsoft.EntityFrameworkCore;

namespace ControlAcceso.Persistencia.Repositorios
{
    public sealed class RepositorioPuntosAcceso : Repositorio<PuntoAcceso>, IRepositorioPuntosAcceso
    {
        public RepositorioPuntosAcceso(ContextoDatos contexto) : base(contexto)
        {
        }

        public async Task<RespuestaPaginada<PuntoAcceso>> ObtenerPaginadoAsync(int numeroPagina,
                                                                               int tamanoPagina,
                                                                               bool? habilitado,
                                                                               TipoPuntoAcceso? tipo,
                                                                               CancellationToken cancelacion = default)
        {
            IQueryable<PuntoAcceso> consulta = _contexto.PuntosAcceso.AsNoTracking();

            if (habilitado.HasValue)
            {
                consulta = consulta.Where(puntoAcceso => puntoAcceso.Habilitado == habilitado.Value);
            }

            if (tipo.HasValue)
            {
                consulta = consulta.Where(puntoAcceso => puntoAcceso.Tipo == tipo.Value);
            }

            consulta = consulta.OrderBy(puntoAcceso => puntoAcceso.Nombre);

            return await consulta.APaginaAsync(numeroPagina, tamanoPagina, cancelacion);
        }

        public async Task<bool> ExistePorNombreAsync(string nombre, CancellationToken cancelacion = default)
        {
            string nombreDepurado = nombre.Trim();

            return await _contexto.PuntosAcceso.AnyAsync(puntoAcceso => puntoAcceso.Nombre == nombreDepurado, cancelacion);
        }
    }
}
