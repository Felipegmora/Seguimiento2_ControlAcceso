using ControlAcceso.Aplicacion.Utilidades.Paginacion;
using Microsoft.EntityFrameworkCore;

namespace ControlAcceso.Persistencia.Extensiones
{
    internal static class ExtensionesConsulta
    {
        public static async Task<RespuestaPaginada<TEntidad>> APaginaAsync<TEntidad>(this IQueryable<TEntidad> consulta,
                                                                                     int numeroPagina,
                                                                                     int tamanoPagina,
                                                                                     CancellationToken cancelacion)
        {
            int totalRegistros = await consulta.CountAsync(cancelacion);

            List<TEntidad> elementos = await consulta.Skip((numeroPagina - 1) * tamanoPagina)
                                                     .Take(tamanoPagina)
                                                     .ToListAsync(cancelacion);

            return RespuestaPaginada<TEntidad>.Crear(elementos, totalRegistros, numeroPagina, tamanoPagina);
        }
    }
}
