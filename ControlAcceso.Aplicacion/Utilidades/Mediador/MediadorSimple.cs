using System.Reflection;
using ControlAcceso.Aplicacion.Excepciones;
using FluentValidation;
using FluentValidation.Results;

namespace ControlAcceso.Aplicacion.Utilidades.Mediador
{
    public sealed class MediadorSimple : IMediador
    {
        private readonly IServiceProvider _proveedorServicios;

        public MediadorSimple(IServiceProvider proveedorServicios)
        {
            _proveedorServicios = proveedorServicios;
        }

        public async Task<TRespuesta> Enviar<TRespuesta>(IPeticion<TRespuesta> peticion, CancellationToken cancelacion = default)
        {
            ArgumentNullException.ThrowIfNull(peticion);

            await ValidarPeticion(peticion, cancelacion);

            Type tipoManejador = typeof(IManejadorPeticion<,>).MakeGenericType(peticion.GetType(), typeof(TRespuesta));

            object? manejador = _proveedorServicios.GetService(tipoManejador);

            if (manejador is null)
            {
                throw new ExcepcionMediador($"No se encontró un manejador para {peticion.GetType().Name}.");
            }

            MethodInfo metodo = tipoManejador.GetMethod("Manejar")!;

            Task<TRespuesta> tarea = (Task<TRespuesta>)metodo.Invoke(manejador, new object?[] { peticion, cancelacion })!;

            return await tarea;
        }

        private async Task ValidarPeticion(object peticion, CancellationToken cancelacion)
        {
            Type tipoValidador = typeof(IValidator<>).MakeGenericType(peticion.GetType());

            if (_proveedorServicios.GetService(tipoValidador) is not IValidator validador)
            {
                return;
            }

            ValidationResult resultado = await validador.ValidateAsync(new ValidationContext<object>(peticion), cancelacion);

            if (!resultado.IsValid)
            {
                throw new ExcepcionValidacion(resultado.Errors);
            }
        }
    }
}
