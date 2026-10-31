using ControlAcceso.Aplicacion.CasosDeUso.Comun;
using ControlAcceso.Aplicacion.CasosDeUso.Credenciales.Dtos;
using ControlAcceso.Aplicacion.Contratos.Repositorios;
using ControlAcceso.Aplicacion.Excepciones;
using ControlAcceso.Aplicacion.Utilidades.Mediador;
using ControlAcceso.Aplicacion.Utilidades.Resultados;
using ControlAcceso.Dominio.Entidades.Credenciales;
using FluentValidation;

namespace ControlAcceso.Aplicacion.CasosDeUso.Credenciales.Consultas
{
    public sealed record ObtenerCredencialPorCodigoConsulta(string Codigo) : IPeticion<Resultado<CredencialDto>>;

    public sealed class ObtenerCredencialPorCodigoValidador : AbstractValidator<ObtenerCredencialPorCodigoConsulta>
    {
        public ObtenerCredencialPorCodigoValidador()
        {
            RuleFor(consulta => consulta.Codigo)
                .NotEmpty().WithMessage("El código de la credencial es requerido.")
                .MaximumLength(Credencial.LongitudMaximaCodigo)
                .WithMessage($"El código de la credencial debe tener máximo {Credencial.LongitudMaximaCodigo} caracteres.");
        }
    }

    public sealed class ObtenerCredencialPorCodigoManejador : ManejadorBase, IManejadorPeticion<ObtenerCredencialPorCodigoConsulta, Resultado<CredencialDto>>
    {
        private readonly IRepositorioCredenciales _repositorioCredenciales;

        public ObtenerCredencialPorCodigoManejador(IRepositorioCredenciales repositorioCredenciales)
        {
            _repositorioCredenciales = repositorioCredenciales;
        }

        public Task<Resultado<CredencialDto>> Manejar(ObtenerCredencialPorCodigoConsulta peticion, CancellationToken cancelacion = default)
        {
            return Ejecutar(async () =>
            {
                Credencial credencial = await _repositorioCredenciales.ObtenerPorCodigoConEmpleadoAsync(peticion.Codigo, cancelacion)
                    ?? throw new ExcepcionNoEncontrado($"No existe una credencial con el código {Credencial.NormalizarCodigo(peticion.Codigo)}.");

                return CredencialDto.Desde(credencial);
            });
        }
    }
}
