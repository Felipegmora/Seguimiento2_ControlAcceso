using ControlAcceso.Dominio.Entidades.Areas;

namespace ControlAcceso.Aplicacion.CasosDeUso.Areas.Dtos
{
    public sealed record AreaDto(Guid Id, string Nombre, string? Descripcion)
    {
        public static AreaDto Desde(Area area)
        {
            return new AreaDto(area.Id, area.Nombre, area.Descripcion);
        }
    }
}
