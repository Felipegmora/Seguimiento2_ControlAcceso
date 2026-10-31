using FluentValidation.Results;

namespace ControlAcceso.Aplicacion.Excepciones
{
    public sealed class ExcepcionValidacion : Exception
    {
        public IReadOnlyCollection<ValidationFailure> Errores { get; }

        public ExcepcionValidacion(IReadOnlyCollection<ValidationFailure> errores)
            : base("La solicitud no es válida.")
        {
            Errores = errores;
        }
    }
}
