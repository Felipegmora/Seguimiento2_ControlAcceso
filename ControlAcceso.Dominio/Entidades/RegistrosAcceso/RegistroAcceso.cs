using ControlAcceso.Dominio.Entidades.Credenciales;
using ControlAcceso.Dominio.Entidades.Empleados;
using ControlAcceso.Dominio.Entidades.PuntosAcceso;
using ControlAcceso.Dominio.Entidades.RegistrosAcceso.ObjetosDeValor;
using ControlAcceso.Dominio.Excepciones;

namespace ControlAcceso.Dominio.Entidades.RegistrosAcceso
{
    public sealed class RegistroAcceso
    {
        public const int LongitudMaximaMotivo = 200;

        public Guid Id { get; private set; }
        public Guid EmpleadoId { get; private set; }
        public Empleado Empleado { get; private set; } = null!;
        public Guid CredencialId { get; private set; }
        public Credencial Credencial { get; private set; } = null!;
        public Guid PuntoAccesoId { get; private set; }
        public PuntoAcceso PuntoAcceso { get; private set; } = null!;
        public TipoMovimiento Movimiento { get; private set; }
        public ResultadoAcceso Resultado { get; private set; }
        public string? Motivo { get; private set; }
        public DateTime FechaMovimiento { get; private set; }

        private RegistroAcceso()
        {
        }

        private RegistroAcceso(Guid empleadoId,
                               Guid credencialId,
                               Guid puntoAccesoId,
                               TipoMovimiento movimiento,
                               ResultadoAcceso resultado,
                               string? motivo)
        {
            AplicarReglasIdentificador(empleadoId, "El empleado del movimiento es requerido.");
            AplicarReglasIdentificador(credencialId, "La credencial del movimiento es requerida.");
            AplicarReglasIdentificador(puntoAccesoId, "El punto de acceso del movimiento es requerido.");
            AplicarReglasMovimiento(movimiento);
            AplicarReglasMotivo(resultado, motivo);

            Id = Guid.CreateVersion7();
            EmpleadoId = empleadoId;
            CredencialId = credencialId;
            PuntoAccesoId = puntoAccesoId;
            Movimiento = movimiento;
            Resultado = resultado;
            Motivo = motivo?.Trim();
            FechaMovimiento = DateTime.UtcNow;
        }

        public static RegistroAcceso CrearAutorizado(Guid empleadoId,
                                                     Guid credencialId,
                                                     Guid puntoAccesoId,
                                                     TipoMovimiento movimiento)
        {
            return new RegistroAcceso(empleadoId, credencialId, puntoAccesoId, movimiento, ResultadoAcceso.Autorizado, null);
        }

        public static RegistroAcceso CrearRechazado(Guid empleadoId,
                                                    Guid credencialId,
                                                    Guid puntoAccesoId,
                                                    TipoMovimiento movimiento,
                                                    string motivo)
        {
            return new RegistroAcceso(empleadoId, credencialId, puntoAccesoId, movimiento, ResultadoAcceso.Rechazado, motivo);
        }

        private static void AplicarReglasIdentificador(Guid identificador, string mensaje)
        {
            if (identificador == Guid.Empty)
            {
                throw new ExcepcionReglaNegocio(mensaje);
            }
        }

        private static void AplicarReglasMovimiento(TipoMovimiento movimiento)
        {
            if (!Enum.IsDefined(movimiento))
            {
                throw new ExcepcionReglaNegocio("El tipo de movimiento no es válido.");
            }
        }

        private static void AplicarReglasMotivo(ResultadoAcceso resultado, string? motivo)
        {
            if (resultado == ResultadoAcceso.Rechazado && string.IsNullOrWhiteSpace(motivo))
            {
                throw new ExcepcionReglaNegocio("Un movimiento rechazado debe indicar el motivo.");
            }

            if (motivo is not null && motivo.Trim().Length > LongitudMaximaMotivo)
            {
                throw new ExcepcionReglaNegocio($"El motivo debe tener máximo {LongitudMaximaMotivo} caracteres.");
            }
        }
    }
}
