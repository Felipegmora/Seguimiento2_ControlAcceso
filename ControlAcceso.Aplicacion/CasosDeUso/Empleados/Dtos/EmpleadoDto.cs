using ControlAcceso.Dominio.Entidades.Empleados;
using ControlAcceso.Dominio.Entidades.Empleados.ObjetosDeValor;

namespace ControlAcceso.Aplicacion.CasosDeUso.Empleados.Dtos
{
    public sealed record EmpleadoDto(Guid Id,
                                     string NumeroDocumento,
                                     string Nombres,
                                     string Apellidos,
                                     string CorreoElectronico,
                                     string Cargo,
                                     Guid AreaId,
                                     string Area,
                                     EstadoLaboral Estado,
                                     DateTime FechaCreacion,
                                     DateTime? FechaDesactivacion)
    {
        public static EmpleadoDto Desde(Empleado empleado)
        {
            return new EmpleadoDto(empleado.Id,
                                   empleado.NumeroDocumento.Valor,
                                   empleado.Nombres,
                                   empleado.Apellidos,
                                   empleado.CorreoElectronico.Valor,
                                   empleado.Cargo,
                                   empleado.AreaId,
                                   empleado.Area is null ? string.Empty : empleado.Area.Nombre,
                                   empleado.Estado,
                                   empleado.FechaCreacion,
                                   empleado.FechaDesactivacion);
        }
    }
}
