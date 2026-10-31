using ControlAcceso.Aplicacion.CasosDeUso.Comun;
using ControlAcceso.Aplicacion.CasosDeUso.RegistrosAcceso.Dtos;
using ControlAcceso.Aplicacion.Contratos.Persistencia;
using ControlAcceso.Aplicacion.Contratos.Repositorios;
using ControlAcceso.Aplicacion.Excepciones;
using ControlAcceso.Aplicacion.Utilidades.Mediador;
using ControlAcceso.Aplicacion.Utilidades.Resultados;
using ControlAcceso.Dominio.Entidades.Credenciales;
using ControlAcceso.Dominio.Entidades.Credenciales.ObjetosDeValor;
using ControlAcceso.Dominio.Entidades.PuntosAcceso;
using ControlAcceso.Dominio.Entidades.RegistrosAcceso;
using ControlAcceso.Dominio.Entidades.RegistrosAcceso.ObjetosDeValor;
using FluentValidation;

namespace ControlAcceso.Aplicacion.CasosDeUso.RegistrosAcceso.Comandos
{
    public sealed record RegistrarMovimientoComando(string CodigoCredencial,
                                                    Guid PuntoAccesoId,
                                                    TipoMovimiento Movimiento) : IPeticion<Resultado<ResultadoMovimientoDto>>;

    public sealed class RegistrarMovimientoValidador : AbstractValidator<RegistrarMovimientoComando>
    {
        public RegistrarMovimientoValidador()
        {
            RuleFor(comando => comando.CodigoCredencial)
                .NotEmpty().WithMessage("El código de la credencial es requerido.")
                .MaximumLength(Credencial.LongitudMaximaCodigo)
                .WithMessage($"El código de la credencial debe tener máximo {Credencial.LongitudMaximaCodigo} caracteres.");

            RuleFor(comando => comando.PuntoAccesoId)
                .NotEmpty().WithMessage("El punto de acceso es requerido.");

            RuleFor(comando => comando.Movimiento)
                .IsInEnum().WithMessage("El tipo de movimiento no es válido.");
        }
    }

    public sealed class RegistrarMovimientoManejador : ManejadorBase, IManejadorPeticion<RegistrarMovimientoComando, Resultado<ResultadoMovimientoDto>>
    {
        private readonly IRepositorioCredenciales _repositorioCredenciales;
        private readonly IRepositorioPuntosAcceso _repositorioPuntosAcceso;
        private readonly IRepositorioRegistrosAcceso _repositorioRegistrosAcceso;
        private readonly IUnidadDeTrabajo _unidadDeTrabajo;

        public RegistrarMovimientoManejador(IRepositorioCredenciales repositorioCredenciales,
                                            IRepositorioPuntosAcceso repositorioPuntosAcceso,
                                            IRepositorioRegistrosAcceso repositorioRegistrosAcceso,
                                            IUnidadDeTrabajo unidadDeTrabajo)
        {
            _repositorioCredenciales = repositorioCredenciales;
            _repositorioPuntosAcceso = repositorioPuntosAcceso;
            _repositorioRegistrosAcceso = repositorioRegistrosAcceso;
            _unidadDeTrabajo = unidadDeTrabajo;
        }

        public Task<Resultado<ResultadoMovimientoDto>> Manejar(RegistrarMovimientoComando peticion, CancellationToken cancelacion = default)
        {
            return Ejecutar(async () =>
            {
                Credencial credencial = await _repositorioCredenciales.ObtenerPorCodigoConEmpleadoAsync(peticion.CodigoCredencial, cancelacion)
                    ?? throw new ExcepcionNoEncontrado($"No existe una credencial con el código {Credencial.NormalizarCodigo(peticion.CodigoCredencial)}.");

                PuntoAcceso puntoAcceso = await _repositorioPuntosAcceso.ObtenerPorIdAsync(peticion.PuntoAccesoId, cancelacion)
                    ?? throw new ExcepcionNoEncontrado($"No existe un punto de acceso con el identificador {peticion.PuntoAccesoId}.");

                RegistroAcceso? ultimoMovimiento = await _repositorioRegistrosAcceso
                    .ObtenerUltimoAutorizadoPorEmpleadoAsync(credencial.EmpleadoId, cancelacion);

                string? motivoRechazo = DeterminarMotivoRechazo(credencial, puntoAcceso, ultimoMovimiento, peticion.Movimiento);

                RegistroAcceso registro = motivoRechazo is null
                    ? RegistroAcceso.CrearAutorizado(credencial.EmpleadoId, credencial.Id, puntoAcceso.Id, peticion.Movimiento)
                    : RegistroAcceso.CrearRechazado(credencial.EmpleadoId, credencial.Id, puntoAcceso.Id, peticion.Movimiento, motivoRechazo);

                await _repositorioRegistrosAcceso.AgregarAsync(registro, cancelacion);
                await _unidadDeTrabajo.ConfirmarAsync(cancelacion);

                return ResultadoMovimientoDto.Desde(registro);
            }, "Movimiento registrado con éxito");
        }

        private static string? DeterminarMotivoRechazo(Credencial credencial,
                                                       PuntoAcceso puntoAcceso,
                                                       RegistroAcceso? ultimoMovimiento,
                                                       TipoMovimiento movimiento)
        {
            if (!credencial.PuedeUtilizarse)
            {
                return credencial.Estado == EstadoCredencial.Bloqueada
                    ? "La credencial se encuentra bloqueada."
                    : "La credencial se encuentra revocada.";
            }

            if (credencial.Empleado is null || !credencial.Empleado.EstaActivo)
            {
                return "El empleado no tiene un vínculo laboral activo.";
            }

            if (!puntoAcceso.Habilitado)
            {
                return "El punto de acceso se encuentra deshabilitado.";
            }

            if (movimiento == TipoMovimiento.Entrada && ultimoMovimiento?.Movimiento == TipoMovimiento.Entrada)
            {
                return "El empleado ya registra una entrada sin salida.";
            }

            if (movimiento == TipoMovimiento.Salida && ultimoMovimiento?.Movimiento != TipoMovimiento.Entrada)
            {
                return "El empleado no registra una entrada previa.";
            }

            return null;
        }
    }
}
