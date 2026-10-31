namespace ControlAcceso.Aplicacion.Utilidades.Paginacion
{
    public sealed class RespuestaPaginada<T>
    {
        public const int NumeroPaginaPredeterminado = 1;
        public const int TamanoPaginaPredeterminado = 15;
        public const int TamanoPaginaMaximo = 50;

        public required List<T> Elementos { get; init; }
        public required int TotalRegistros { get; init; }
        public required int NumeroPagina { get; init; }
        public required int TamanoPagina { get; init; }

        public int TotalPaginas => TamanoPagina <= 0 ? 0 : (int)Math.Ceiling(TotalRegistros / (double)TamanoPagina);
        public bool TienePaginaAnterior => NumeroPagina > 1;
        public bool TienePaginaSiguiente => NumeroPagina < TotalPaginas;

        public static RespuestaPaginada<T> Crear(List<T> elementos, int totalRegistros, int numeroPagina, int tamanoPagina)
        {
            return new RespuestaPaginada<T>
            {
                Elementos = elementos,
                TotalRegistros = totalRegistros,
                NumeroPagina = numeroPagina,
                TamanoPagina = tamanoPagina
            };
        }
    }
}
