using ControlAcceso.Aplicacion.CasosDeUso.Comun;
using ControlAcceso.Aplicacion.Contratos.Persistencia;
using ControlAcceso.Aplicacion.Contratos.Repositorios;
using ControlAcceso.Aplicacion.Excepciones;
using ControlAcceso.Aplicacion.Utilidades.Mediador;
using ControlAcceso.Aplicacion.Utilidades.Resultados;
using ControlAcceso.Dominio.Entidades.Credenciales;
using ControlAcceso.Dominio.Entidades.Empleados;
using ControlAcceso.Dominio.Excepciones;
using FluentValidation;

namespace ControlAcceso.Aplicacion.CasosDeUso.Credenciales.Comandos
{
    public sealed record AsignarCredencialComando(string Codigo, Guid EmpleadoId) : IPeticion<Resultado<Guid>>;

    public sealed class AsignarCredencialValidador : AbstractValidator<AsignarCredencialComando>
    {
        public AsignarCredencialValidador()
        {
            RuleFor(comando => comando.Codigo)
                .NotEmpty().WithMessage("El código de la credencial es requerido.")
                .MaximumLength(Credencial.LongitudMaximaCodigo)
                .WithMessage($"El código de la credencial debe tener máximo {Credencial.LongitudMaximaCodigo} caracteres.");

            RuleFor(comando => comando.EmpleadoId)
                .NotEmpty().WithMessage("El empleado de la credencial es requerido.");
        }
    }

    public sealed class AsignarCredencialManejador : ManejadorBase, IManejadorPeticion<AsignarCredencialComando, Resultado<Guid>>
    {
        private readonly IRepositorioCredenciales _repositorioCredenciales;
        private readonly IRepositorioEmpleados _repositorioEmpleados;
        private readonly IUnidadDeTrabajo _unidadDeTrabajo;

        public AsignarCredencialManejador(IRepositorioCredenciales repositorioCredenciales,
                                          IRepositorioEmpleados repositorioEmpleados,
                                          IUnidadDeTrabajo unidadDeTrabajo)
        {
            _repositorioCredenciales = repositorioCredenciales;
            _repositorioEmpleados = repositorioEmpleados;
            _unidadDeTrabajo = unidadDeTrabajo;
        }

        public Task<Resultado<Guid>> Manejar(AsignarCredencialComando peticion, CancellationToken cancelacion = default)
        {
            return Ejecutar(async () =>
            {
                Empleado empleado = await _repositorioEmpleados.ObtenerPorIdAsync(peticion.EmpleadoId, cancelacion)
                    ?? throw new ExcepcionNoEncontrado($"No existe un empleado con el identificador {peticion.EmpleadoId}.");

                if (!empleado.EstaActivo)
                {
                    throw new ExcepcionReglaNegocio("No se puede asignar una credencial a un empleado inactivo.");
                }

                if (await _repositorioCredenciales.ExistePorCodigoAsync(peticion.Codigo, cancelacion))
                {
                    throw new ExcepcionReglaNegocio($"Ya existe una credencial con el código {Credencial.NormalizarCodigo(peticion.Codigo)}.");
                }

                Credencial credencial = new(peticion.Codigo, peticion.EmpleadoId);

                await _repositorioCredenciales.AgregarAsync(credencial, cancelacion);
                await _unidadDeTrabajo.ConfirmarAsync(cancelacion);

                return credencial.Id;
            }, "Credencial asignada con éxito");
        }
    }
}
