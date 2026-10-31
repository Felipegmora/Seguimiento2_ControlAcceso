namespace ControlAcceso.Aplicacion.Utilidades.Mediador
{
    public interface IManejadorPeticion<TPeticion, TRespuesta> where TPeticion : IPeticion<TRespuesta>
    {
        Task<TRespuesta> Manejar(TPeticion peticion, CancellationToken cancelacion = default);
    }
}
