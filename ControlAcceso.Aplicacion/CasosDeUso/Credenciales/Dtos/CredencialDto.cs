using ControlAcceso.Dominio.Entidades.Credenciales;
using ControlAcceso.Dominio.Entidades.Credenciales.ObjetosDeValor;

namespace ControlAcceso.Aplicacion.CasosDeUso.Credenciales.Dtos
{
    public sealed record CredencialDto(Guid Id,
                                       string Codigo,
                                       Guid EmpleadoId,
                                       string Empleado,
                                       EstadoCredencial Estado,
                                       bool PuedeUtilizarse,
                                       DateTime FechaCreacion,
                                       DateTime? FechaBloqueo,
                                       DateTime? FechaRevocacion)
    {
        public static CredencialDto Desde(Credencial credencial)
        {
            string nombreEmpleado = credencial.Empleado is null
                ? string.Empty
                : $"{credencial.Empleado.Nombres} {credencial.Empleado.Apellidos}";

            return new CredencialDto(credencial.Id,
                                     credencial.Codigo,
                                     credencial.EmpleadoId,
                                     nombreEmpleado,
                                     credencial.Estado,
                                     credencial.PuedeUtilizarse,
                                     credencial.FechaCreacion,
                                     credencial.FechaBloqueo,
                                     credencial.FechaRevocacion);
        }
    }
}
