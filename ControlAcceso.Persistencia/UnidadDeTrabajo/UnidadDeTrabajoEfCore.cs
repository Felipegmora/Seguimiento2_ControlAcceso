using ControlAcceso.Aplicacion.Contratos.Persistencia;

namespace ControlAcceso.Persistencia.UnidadDeTrabajo
{
    public sealed class UnidadDeTrabajoEfCore : IUnidadDeTrabajo
    {
        private readonly ContextoDatos _contexto;

        public UnidadDeTrabajoEfCore(ContextoDatos contexto)
        {
            _contexto = contexto;
        }

        public async Task ConfirmarAsync(CancellationToken cancelacion = default)
        {
            await _contexto.SaveChangesAsync(cancelacion);
        }
    }
}
