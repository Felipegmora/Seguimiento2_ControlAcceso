namespace ControlAcceso.Api.Respuestas
{
    public sealed class RespuestaApi
    {
        public bool Exitoso { get; init; }
        public string? Mensaje { get; init; }
        public IReadOnlyCollection<ErrorApi> Errores { get; init; } = [];

        public static RespuestaApi Correcta(string? mensaje = null)
        {
            return new RespuestaApi
            {
                Exitoso = true,
                Mensaje = mensaje
            };
        }

        public static RespuestaApi Fallida(IReadOnlyCollection<ErrorApi> errores, string? mensaje = null)
        {
            return new RespuestaApi
            {
                Exitoso = false,
                Mensaje = mensaje ?? "Ha ocurrido un error",
                Errores = errores
            };
        }

        public static RespuestaApi Fallida(ErrorApi error, string? mensaje = null)
        {
            return Fallida([error], mensaje);
        }
    }

    public sealed class RespuestaApi<T>
    {
        public bool Exitoso { get; init; }
        public string? Mensaje { get; init; }
        public T? Datos { get; init; }
        public IReadOnlyCollection<ErrorApi> Errores { get; init; } = [];

        public static RespuestaApi<T> Correcta(T? datos, string? mensaje = null)
        {
            return new RespuestaApi<T>
            {
                Exitoso = true,
                Mensaje = mensaje,
                Datos = datos
            };
        }

        public static RespuestaApi<T> Fallida(IReadOnlyCollection<ErrorApi> errores, string? mensaje = null)
        {
            return new RespuestaApi<T>
            {
                Exitoso = false,
                Mensaje = mensaje ?? "Ha ocurrido un error",
                Errores = errores
            };
        }
    }
}
