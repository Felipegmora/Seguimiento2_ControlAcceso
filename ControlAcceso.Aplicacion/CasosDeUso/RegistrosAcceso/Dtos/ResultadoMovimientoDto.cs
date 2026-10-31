using ControlAcceso.Dominio.Entidades.RegistrosAcceso;
using ControlAcceso.Dominio.Entidades.RegistrosAcceso.ObjetosDeValor;

namespace ControlAcceso.Aplicacion.CasosDeUso.RegistrosAcceso.Dtos
{
    public sealed record ResultadoMovimientoDto(Guid RegistroId,
                                                TipoMovimiento Movimiento,
                                                ResultadoAcceso Resultado,
                                                string? Motivo,
                                                DateTime FechaMovimiento)
    {
        public static ResultadoMovimientoDto Desde(RegistroAcceso registro)
        {
            return new ResultadoMovimientoDto(registro.Id,
                                              registro.Movimiento,
                                              registro.Resultado,
                                              registro.Motivo,
                                              registro.FechaMovimiento);
        }
    }
}
