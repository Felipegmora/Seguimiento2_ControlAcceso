namespace ControlAcceso.Aplicacion.Contratos.Persistencia
{
    public interface IUnidadDeTrabajo
    {
        Task ConfirmarAsync(CancellationToken cancelacion = default);
    }
}
