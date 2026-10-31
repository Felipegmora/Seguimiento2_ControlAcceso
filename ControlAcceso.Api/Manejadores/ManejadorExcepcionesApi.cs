using ControlAcceso.Api.Mapeadores;
using ControlAcceso.Api.Respuestas;
using Microsoft.AspNetCore.Diagnostics;

namespace ControlAcceso.Api.Manejadores
{
    public sealed class ManejadorExcepcionesApi : IExceptionHandler
    {
        private readonly ILogger<ManejadorExcepcionesApi> _registrador;

        public ManejadorExcepcionesApi(ILogger<ManejadorExcepcionesApi> registrador)
        {
            _registrador = registrador;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext contexto,
                                                    Exception excepcion,
                                                    CancellationToken cancelacion)
        {
            (int codigoEstado, RespuestaApi respuesta) = MapeadorErroresApi.MapearExcepcion(excepcion);

            if (codigoEstado == StatusCodes.Status500InternalServerError)
            {
                _registrador.LogError(excepcion,
                                      "Error no controlado en {Metodo} {Ruta}",
                                      contexto.Request.Method,
                                      contexto.Request.Path);
            }

            contexto.Response.StatusCode = codigoEstado;

            await contexto.Response.WriteAsJsonAsync(respuesta, cancelacion);

            return true;
        }
    }
}
