using ControlAcceso.Api.Respuestas;
using ControlAcceso.Aplicacion.Excepciones;
using ControlAcceso.Aplicacion.Utilidades.Resultados;
using ControlAcceso.Dominio.Excepciones;

namespace ControlAcceso.Api.Mapeadores
{
    public static class MapeadorErroresApi
    {
        public static int ObtenerCodigoEstado(TipoError tipo)
        {
            return tipo switch
            {
                TipoError.Validacion => StatusCodes.Status400BadRequest,
                TipoError.NoEncontrado => StatusCodes.Status404NotFound,
                TipoError.ReglaNegocio => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            };
        }

        public static int ObtenerCodigoEstado(IReadOnlyCollection<Error> errores)
        {
            if (errores.Count == 0)
            {
                return StatusCodes.Status500InternalServerError;
            }

            TipoError[] prioridad =
            [
                TipoError.Inesperado,
                TipoError.Validacion,
                TipoError.ReglaNegocio,
                TipoError.NoEncontrado
            ];

            foreach (TipoError tipo in prioridad)
            {
                if (errores.Any(error => error.Tipo == tipo))
                {
                    return ObtenerCodigoEstado(tipo);
                }
            }

            return StatusCodes.Status500InternalServerError;
        }

        public static IReadOnlyCollection<ErrorApi> Mapear(IEnumerable<Error> errores)
        {
            return errores.Select(error => new ErrorApi(error.Codigo, error.Descripcion, error.Tipo.ToString())).ToArray();
        }

        public static (int CodigoEstado, RespuestaApi Respuesta) MapearExcepcion(Exception excepcion)
        {
            switch (excepcion)
            {
                case ExcepcionValidacion excepcionValidacion:
                {
                    ErrorApi[] errores = excepcionValidacion.Errores
                        .Select(fallo => new ErrorApi($"Validacion.{fallo.PropertyName}",
                                                      fallo.ErrorMessage,
                                                      TipoError.Validacion.ToString()))
                        .ToArray();

                    return (StatusCodes.Status400BadRequest, RespuestaApi.Fallida(errores, "La solicitud no es válida"));
                }

                case ExcepcionNoEncontrado excepcionNoEncontrado:
                {
                    ErrorApi error = new("Negocio.NoEncontrado",
                                         excepcionNoEncontrado.Message,
                                         TipoError.NoEncontrado.ToString());

                    return (StatusCodes.Status404NotFound, RespuestaApi.Fallida(error, "Recurso no encontrado"));
                }

                case ExcepcionReglaNegocio excepcionReglaNegocio:
                {
                    ErrorApi error = new("Negocio.ReglaViolada",
                                         excepcionReglaNegocio.Message,
                                         TipoError.ReglaNegocio.ToString());

                    return (StatusCodes.Status409Conflict, RespuestaApi.Fallida(error, "La operación no cumple una regla de negocio"));
                }

                case ExcepcionMediador excepcionMediador:
                {
                    ErrorApi error = new("Mediador.ManejadorNoRegistrado",
                                         excepcionMediador.Message,
                                         TipoError.Inesperado.ToString());

                    return (StatusCodes.Status500InternalServerError, RespuestaApi.Fallida(error, "El caso de uso no está registrado"));
                }

                default:
                {
                    ErrorApi error = new("Inesperado",
                                         "Ocurrió un error inesperado.",
                                         TipoError.Inesperado.ToString());

                    return (StatusCodes.Status500InternalServerError, RespuestaApi.Fallida(error, "Ha ocurrido un error"));
                }
            }
        }
    }
}
