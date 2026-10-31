using ControlAcceso.Dominio.Excepciones;

namespace ControlAcceso.Dominio.Entidades.Areas
{
    public sealed class Area
    {
        public const int LongitudMinimaNombre = 2;
        public const int LongitudMaximaNombre = 100;
        public const int LongitudMaximaDescripcion = 500;

        public Guid Id { get; private set; }
        public string Nombre { get; private set; } = null!;
        public string? Descripcion { get; private set; }

        private Area()
        {
        }

        public Area(string nombre, string? descripcion = null)
        {
            AplicarReglasNombre(nombre);
            AplicarReglasDescripcion(descripcion);

            Id = Guid.CreateVersion7();
            Nombre = nombre.Trim();
            Descripcion = descripcion?.Trim();
        }

        private static void AplicarReglasNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ExcepcionReglaNegocio("El nombre del área es requerido.");
            }

            if (nombre.Trim().Length < LongitudMinimaNombre || nombre.Trim().Length > LongitudMaximaNombre)
            {
                throw new ExcepcionReglaNegocio($"El nombre del área debe tener entre {LongitudMinimaNombre} y {LongitudMaximaNombre} caracteres.");
            }
        }

        private static void AplicarReglasDescripcion(string? descripcion)
        {
            if (descripcion is not null && descripcion.Trim().Length > LongitudMaximaDescripcion)
            {
                throw new ExcepcionReglaNegocio($"La descripción del área debe tener máximo {LongitudMaximaDescripcion} caracteres.");
            }
        }
    }
}
