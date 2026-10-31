using ControlAcceso.Dominio.Entidades.PuntosAcceso.ObjetosDeValor;
using ControlAcceso.Dominio.Excepciones;

namespace ControlAcceso.Dominio.Entidades.PuntosAcceso
{
    public sealed class PuntoAcceso
    {
        public const int LongitudMinimaNombre = 2;
        public const int LongitudMaximaNombre = 100;
        public const int LongitudMaximaDescripcion = 500;

        public Guid Id { get; private set; }
        public string Nombre { get; private set; } = null!;
        public TipoPuntoAcceso Tipo { get; private set; }
        public bool Habilitado { get; private set; }
        public string? Descripcion { get; private set; }

        private PuntoAcceso()
        {
        }

        public PuntoAcceso(string nombre, TipoPuntoAcceso tipo, string? descripcion = null)
        {
            AplicarReglasNombre(nombre);
            AplicarReglasTipo(tipo);
            AplicarReglasDescripcion(descripcion);

            Id = Guid.CreateVersion7();
            Nombre = nombre.Trim();
            Tipo = tipo;
            Descripcion = descripcion?.Trim();
            Habilitado = true;
        }

        public void Habilitar()
        {
            if (Habilitado)
            {
                throw new ExcepcionReglaNegocio("El punto de acceso ya se encuentra habilitado.");
            }

            Habilitado = true;
        }

        public void Deshabilitar()
        {
            if (!Habilitado)
            {
                throw new ExcepcionReglaNegocio("El punto de acceso ya se encuentra deshabilitado.");
            }

            Habilitado = false;
        }

        private static void AplicarReglasNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ExcepcionReglaNegocio("El nombre del punto de acceso es requerido.");
            }

            if (nombre.Trim().Length < LongitudMinimaNombre || nombre.Trim().Length > LongitudMaximaNombre)
            {
                throw new ExcepcionReglaNegocio($"El nombre del punto de acceso debe tener entre {LongitudMinimaNombre} y {LongitudMaximaNombre} caracteres.");
            }
        }

        private static void AplicarReglasTipo(TipoPuntoAcceso tipo)
        {
            if (!Enum.IsDefined(tipo))
            {
                throw new ExcepcionReglaNegocio("El tipo del punto de acceso no es válido.");
            }
        }

        private static void AplicarReglasDescripcion(string? descripcion)
        {
            if (descripcion is not null && descripcion.Trim().Length > LongitudMaximaDescripcion)
            {
                throw new ExcepcionReglaNegocio($"La descripción del punto de acceso debe tener máximo {LongitudMaximaDescripcion} caracteres.");
            }
        }
    }
}
