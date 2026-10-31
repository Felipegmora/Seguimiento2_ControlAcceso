using ControlAcceso.Dominio.Entidades.Credenciales.ObjetosDeValor;
using ControlAcceso.Dominio.Entidades.Empleados;
using ControlAcceso.Dominio.Excepciones;

namespace ControlAcceso.Dominio.Entidades.Credenciales
{
    public sealed class Credencial
    {
        public const int LongitudMaximaCodigo = 50;

        public Guid Id { get; private set; }
        public string Codigo { get; private set; } = null!;
        public Guid EmpleadoId { get; private set; }
        public Empleado Empleado { get; private set; } = null!;
        public EstadoCredencial Estado { get; private set; }
        public DateTime FechaCreacion { get; private set; }
        public DateTime? FechaBloqueo { get; private set; }
        public DateTime? FechaRevocacion { get; private set; }

        public bool PuedeUtilizarse => Estado == EstadoCredencial.Activa;

        private Credencial()
        {
        }

        public Credencial(string codigo, Guid empleadoId)
        {
            AplicarReglasCodigo(codigo);
            AplicarReglasEmpleado(empleadoId);

            Id = Guid.CreateVersion7();
            Codigo = NormalizarCodigo(codigo);
            EmpleadoId = empleadoId;
            Estado = EstadoCredencial.Activa;
            FechaCreacion = DateTime.UtcNow;
        }

        public void Bloquear()
        {
            if (Estado != EstadoCredencial.Activa)
            {
                throw new ExcepcionReglaNegocio("Solo se puede bloquear una credencial activa.");
            }

            Estado = EstadoCredencial.Bloqueada;
            FechaBloqueo = DateTime.UtcNow;
        }

        public void Revocar()
        {
            if (Estado == EstadoCredencial.Revocada)
            {
                throw new ExcepcionReglaNegocio("La credencial ya se encuentra revocada.");
            }

            Estado = EstadoCredencial.Revocada;
            FechaRevocacion = DateTime.UtcNow;
        }

        public static string NormalizarCodigo(string codigo)
        {
            return string.IsNullOrWhiteSpace(codigo) ? string.Empty : codigo.Trim().ToUpperInvariant();
        }

        private static void AplicarReglasCodigo(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
            {
                throw new ExcepcionReglaNegocio("El código de la credencial es requerido.");
            }

            if (codigo.Trim().Length > LongitudMaximaCodigo)
            {
                throw new ExcepcionReglaNegocio($"El código de la credencial debe tener máximo {LongitudMaximaCodigo} caracteres.");
            }
        }

        private static void AplicarReglasEmpleado(Guid empleadoId)
        {
            if (empleadoId == Guid.Empty)
            {
                throw new ExcepcionReglaNegocio("El empleado de la credencial es requerido.");
            }
        }
    }
}
