namespace ControlAcceso.Aplicacion.Utilidades.Mediador
{
    public interface IMediador
    {
        Task<TRespuesta> Enviar<TRespuesta>(IPeticion<TRespuesta> peticion, CancellationToken cancelacion = default);
    }
}
