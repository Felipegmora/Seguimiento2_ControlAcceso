namespace ControlAcceso.Dominio.Excepciones
{
    public sealed class ExcepcionReglaNegocio : Exception
    {
        public ExcepcionReglaNegocio(string mensaje) : base(mensaje)
        {
        }
    }
}
