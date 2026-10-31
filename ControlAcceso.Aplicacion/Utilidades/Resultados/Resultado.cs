namespace ControlAcceso.Aplicacion.Utilidades.Resultados
{
    public class Resultado
    {
        private readonly List<Error> _errores = [];

        public bool EsExitoso => _errores.Count == 0;
        public bool EsFallido => !EsExitoso;
        public string? Mensaje { get; }
        public IReadOnlyCollection<Error> Errores => _errores;

        protected Resultado(string? mensaje)
        {
            Mensaje = mensaje;
        }

        protected Resultado(IEnumerable<Error> errores, string? mensaje)
        {
            _errores.AddRange(errores);
            Mensaje = mensaje;
        }

        public static Resultado Exitoso(string? mensaje = "Operación realizada con éxito")
        {
            return new Resultado(mensaje);
        }

        public static Resultado Fallido(Error error, string? mensaje = "Ha ocurrido un error")
        {
            return new Resultado([error], mensaje);
        }

        public static Resultado Fallido(IEnumerable<Error> errores, string? mensaje = "Ha ocurrido un error")
        {
            return new Resultado(errores, mensaje);
        }
    }

    public sealed class Resultado<T> : Resultado
    {
        public T? Valor { get; }

        private Resultado(T valor, string? mensaje) : base(mensaje)
        {
            Valor = valor;
        }

        private Resultado(IEnumerable<Error> errores, string? mensaje) : base(errores, mensaje)
        {
        }

        public static Resultado<T> Exitoso(T valor, string? mensaje = "Operación realizada con éxito")
        {
            return new Resultado<T>(valor, mensaje);
        }

        public static new Resultado<T> Fallido(Error error, string? mensaje = "Ha ocurrido un error")
        {
            return new Resultado<T>([error], mensaje);
        }

        public static new Resultado<T> Fallido(IEnumerable<Error> errores, string? mensaje = "Ha ocurrido un error")
        {
            return new Resultado<T>(errores, mensaje);
        }
    }
}
