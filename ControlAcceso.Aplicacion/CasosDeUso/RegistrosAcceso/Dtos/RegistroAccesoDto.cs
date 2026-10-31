using ControlAcceso.Dominio.Entidades.RegistrosAcceso;
using ControlAcceso.Dominio.Entidades.RegistrosAcceso.ObjetosDeValor;

namespace ControlAcceso.Aplicacion.CasosDeUso.RegistrosAcceso.Dtos
{
    public sealed record RegistroAccesoDto(Guid Id,
                                           Guid EmpleadoId,
                                           string Empleado,
                                           Guid CredencialId,
                                           string CodigoCredencial,
                                           Guid PuntoAccesoId,
                                           string PuntoAcceso,
                                           TipoMovimiento Movimiento,
                                           ResultadoAcceso Resultado,
                                           string? Motivo,
                                           DateTime FechaMovimiento)
    {
        public static RegistroAccesoDto Desde(RegistroAcceso registro)
        {
            string nombreEmpleado = registro.Empleado is null
                ? string.Empty
                : $"{registro.Empleado.Nombres} {registro.Empleado.Apellidos}";

            return new RegistroAccesoDto(registro.Id,
                                         registro.EmpleadoId,
                                         nombreEmpleado,
                                         registro.CredencialId,
                                         registro.Credencial is null ? string.Empty : registro.Credencial.Codigo,
                                         registro.PuntoAccesoId,
                                         registro.PuntoAcceso is null ? string.Empty : registro.PuntoAcceso.Nombre,
                                         registro.Movimiento,
                                         registro.Resultado,
                                         registro.Motivo,
                                         registro.FechaMovimiento);
        }
    }
}
