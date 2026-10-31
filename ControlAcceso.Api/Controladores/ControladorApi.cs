using ControlAcceso.Api.Mapeadores;
using ControlAcceso.Api.Respuestas;
using ControlAcceso.Aplicacion.Utilidades.Mediador;
using ControlAcceso.Aplicacion.Utilidades.Resultados;
using Microsoft.AspNetCore.Mvc;

namespace ControlAcceso.Api.Controladores
{
    [ApiController]
    [Produces("application/json")]
    public abstract class ControladorApi : ControllerBase
    {
        protected readonly IMediador _mediador;

        protected ControladorApi(IMediador mediador)
        {
            _mediador = mediador;
        }

        protected IActionResult Responder(Resultado resultado)
        {
            if (resultado.EsFallido)
            {
                return ConstruirFallo(resultado);
            }

            return Ok(RespuestaApi.Correcta(resultado.Mensaje));
        }

        protected IActionResult Responder<T>(Resultado<T> resultado)
        {
            if (resultado.EsFallido)
            {
                return ConstruirFallo<T>(resultado);
            }

            return Ok(RespuestaApi<T>.Correcta(resultado.Valor, resultado.Mensaje));
        }

        protected IActionResult ResponderCreado<T>(Resultado<T> resultado, Func<T, string> construirUbicacion)
        {
            if (resultado.EsFallido)
            {
                return ConstruirFallo<T>(resultado);
            }

            RespuestaApi<T> respuesta = RespuestaApi<T>.Correcta(resultado.Valor, resultado.Mensaje);

            return Created(construirUbicacion(resultado.Valor!), respuesta);
        }

        private IActionResult ConstruirFallo(Resultado resultado)
        {
            int codigoEstado = MapeadorErroresApi.ObtenerCodigoEstado(resultado.Errores);

            return StatusCode(codigoEstado, RespuestaApi.Fallida(MapeadorErroresApi.Mapear(resultado.Errores), resultado.Mensaje));
        }

        private IActionResult ConstruirFallo<T>(Resultado<T> resultado)
        {
            int codigoEstado = MapeadorErroresApi.ObtenerCodigoEstado(resultado.Errores);

            return StatusCode(codigoEstado, RespuestaApi<T>.Fallida(MapeadorErroresApi.Mapear(resultado.Errores), resultado.Mensaje));
        }
    }
}
