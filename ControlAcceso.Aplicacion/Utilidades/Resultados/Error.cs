namespace ControlAcceso.Aplicacion.Utilidades.Resultados
{
    public sealed record Error(string Codigo, string Descripcion, TipoError Tipo);
}
