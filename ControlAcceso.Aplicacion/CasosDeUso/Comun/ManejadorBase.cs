using ControlAcceso.Aplicacion.Excepciones;
using ControlAcceso.Aplicacion.Utilidades.Resultados;
using ControlAcceso.Dominio.Excepciones;

namespace ControlAcceso.Aplicacion.CasosDeUso.Comun
{
    public abstract class ManejadorBase
    {
        protected static async Task<Resultado<T>> Ejecutar<T>(Func<Task<T>> accion, string? mensaje = null)
        {
            try
            {
                T valor = await accion();

                return mensaje is null ? Resultado<T>.Exitoso(valor) : Resultado<T>.Exitoso(valor, mensaje);
            }
            catch (ExcepcionNoEncontrado excepcion)
            {
                return Resultado<T>.Fallido(new Error("Negocio.NoEncontrado", excepcion.Message, TipoError.NoEncontrado));
            }
            catch (ExcepcionReglaNegocio excepcion)
            {
                return Resultado<T>.Fallido(new Error("Negocio.ReglaViolada", excepcion.Message, TipoError.ReglaNegocio));
            }
        }

        protected static async Task<Resultado> Ejecutar(Func<Task> accion, string? mensaje = null)
        {
            try
            {
                await accion();

                return mensaje is null ? Resultado.Exitoso() : Resultado.Exitoso(mensaje);
            }
            catch (ExcepcionNoEncontrado excepcion)
            {
                return Resultado.Fallido(new Error("Negocio.NoEncontrado", excepcion.Message, TipoError.NoEncontrado));
            }
            catch (ExcepcionReglaNegocio excepcion)
            {
                return Resultado.Fallido(new Error("Negocio.ReglaViolada", excepcion.Message, TipoError.ReglaNegocio));
            }
        }
    }
}
