using ControlAcceso.Dominio.Entidades.PuntosAcceso;
using ControlAcceso.Dominio.Entidades.PuntosAcceso.ObjetosDeValor;

namespace ControlAcceso.Aplicacion.CasosDeUso.PuntosAcceso.Dtos
{
    public sealed record PuntoAccesoDto(Guid Id,
                                        string Nombre,
                                        TipoPuntoAcceso Tipo,
                                        bool Habilitado,
                                        string? Descripcion)
    {
        public static PuntoAccesoDto Desde(PuntoAcceso puntoAcceso)
        {
            return new PuntoAccesoDto(puntoAcceso.Id,
                                      puntoAcceso.Nombre,
                                      puntoAcceso.Tipo,
                                      puntoAcceso.Habilitado,
                                      puntoAcceso.Descripcion);
        }
    }
}
