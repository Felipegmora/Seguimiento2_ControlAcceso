using ControlAcceso.Dominio.Entidades.Areas;
using ControlAcceso.Dominio.Entidades.Empleados.ObjetosDeValor;
using ControlAcceso.Dominio.Excepciones;

namespace ControlAcceso.Dominio.Entidades.Empleados
{
    public sealed class Empleado
    {
        public const int LongitudMinimaNombres = 2;
        public const int LongitudMaximaNombres = 100;
        public const int LongitudMinimaCargo = 2;
        public const int LongitudMaximaCargo = 100;

        public Guid Id { get; private set; }
        public NumeroDocumento NumeroDocumento { get; private set; } = null!;
        public string Nombres { get; private set; } = null!;
        public string Apellidos { get; private set; } = null!;
        public CorreoElectronico CorreoElectronico { get; private set; } = null!;
        public string Cargo { get; private set; } = null!;
        public Guid AreaId { get; private set; }
        public Area Area { get; private set; } = null!;
        public EstadoLaboral Estado { get; private set; }
        public DateTime FechaCreacion { get; private set; }
        public DateTime? FechaDesactivacion { get; private set; }

        public bool EstaActivo => Estado == EstadoLaboral.Activo;

        private Empleado()
        {
        }

        public Empleado(NumeroDocumento numeroDocumento,
                        string nombres,
                        string apellidos,
                        CorreoElectronico correoElectronico,
                        string cargo,
                        Guid areaId)
        {
            AplicarReglasNumeroDocumento(numeroDocumento);
            AplicarReglasNombres(nombres);
            AplicarReglasApellidos(apellidos);
            AplicarReglasCorreoElectronico(correoElectronico);
            AplicarReglasCargo(cargo);
            AplicarReglasArea(areaId);

            Id = Guid.CreateVersion7();
            NumeroDocumento = numeroDocumento;
            Nombres = nombres.Trim();
            Apellidos = apellidos.Trim();
            CorreoElectronico = correoElectronico;
            Cargo = cargo.Trim();
            AreaId = areaId;
            Estado = EstadoLaboral.Activo;
            FechaCreacion = DateTime.UtcNow;
        }

        public void Actualizar(string nombres,
                               string apellidos,
                               CorreoElectronico correoElectronico,
                               string cargo,
                               Guid areaId)
        {
            AplicarReglasEmpleadoActivo();
            AplicarReglasNombres(nombres);
            AplicarReglasApellidos(apellidos);
            AplicarReglasCorreoElectronico(correoElectronico);
            AplicarReglasCargo(cargo);
            AplicarReglasArea(areaId);

            Nombres = nombres.Trim();
            Apellidos = apellidos.Trim();
            CorreoElectronico = correoElectronico;
            Cargo = cargo.Trim();
            AreaId = areaId;
        }

        public void Desactivar()
        {
            if (Estado == EstadoLaboral.Inactivo)
            {
                throw new ExcepcionReglaNegocio("El empleado ya se encuentra inactivo.");
            }

            Estado = EstadoLaboral.Inactivo;
            FechaDesactivacion = DateTime.UtcNow;
        }

        private void AplicarReglasEmpleadoActivo()
        {
            if (Estado == EstadoLaboral.Inactivo)
            {
                throw new ExcepcionReglaNegocio("No se puede modificar un empleado inactivo.");
            }
        }

        private static void AplicarReglasNumeroDocumento(NumeroDocumento numeroDocumento)
        {
            if (numeroDocumento is null)
            {
                throw new ExcepcionReglaNegocio("El número de identificación es requerido.");
            }
        }

        private static void AplicarReglasNombres(string nombres)
        {
            if (string.IsNullOrWhiteSpace(nombres))
            {
                throw new ExcepcionReglaNegocio("Los nombres del empleado son requeridos.");
            }

            if (nombres.Trim().Length < LongitudMinimaNombres || nombres.Trim().Length > LongitudMaximaNombres)
            {
                throw new ExcepcionReglaNegocio($"Los nombres del empleado deben tener entre {LongitudMinimaNombres} y {LongitudMaximaNombres} caracteres.");
            }
        }

        private static void AplicarReglasApellidos(string apellidos)
        {
            if (string.IsNullOrWhiteSpace(apellidos))
            {
                throw new ExcepcionReglaNegocio("Los apellidos del empleado son requeridos.");
            }

            if (apellidos.Trim().Length < LongitudMinimaNombres || apellidos.Trim().Length > LongitudMaximaNombres)
            {
                throw new ExcepcionReglaNegocio($"Los apellidos del empleado deben tener entre {LongitudMinimaNombres} y {LongitudMaximaNombres} caracteres.");
            }
        }

        private static void AplicarReglasCorreoElectronico(CorreoElectronico correoElectronico)
        {
            if (correoElectronico is null)
            {
                throw new ExcepcionReglaNegocio("El correo electrónico es requerido.");
            }
        }

        private static void AplicarReglasCargo(string cargo)
        {
            if (string.IsNullOrWhiteSpace(cargo))
            {
                throw new ExcepcionReglaNegocio("El cargo del empleado es requerido.");
            }

            if (cargo.Trim().Length < LongitudMinimaCargo || cargo.Trim().Length > LongitudMaximaCargo)
            {
                throw new ExcepcionReglaNegocio($"El cargo del empleado debe tener entre {LongitudMinimaCargo} y {LongitudMaximaCargo} caracteres.");
            }
        }

        private static void AplicarReglasArea(Guid areaId)
        {
            if (areaId == Guid.Empty)
            {
                throw new ExcepcionReglaNegocio("El área del empleado es requerida.");
            }
        }
    }
}
