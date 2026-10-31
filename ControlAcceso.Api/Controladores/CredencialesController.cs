using ControlAcceso.Aplicacion.CasosDeUso.Credenciales.Comandos;
using ControlAcceso.Aplicacion.CasosDeUso.Credenciales.Consultas;
using ControlAcceso.Aplicacion.CasosDeUso.Credenciales.Dtos;
using ControlAcceso.Aplicacion.Utilidades.Mediador;
using ControlAcceso.Aplicacion.Utilidades.Resultados;
using Microsoft.AspNetCore.Mvc;

namespace ControlAcceso.Api.Controladores
{
    [Route("api/credenciales")]
    public sealed class CredencialesController : ControladorApi
    {
        public CredencialesController(IMediador mediador) : base(mediador)
        {
        }

        [HttpPost]
        public async Task<IActionResult> Asignar([FromBody] AsignarCredencialComando comando, CancellationToken cancelacion)
        {
            Resultado<Guid> resultado = await _mediador.Enviar(comando, cancelacion);

            return ResponderCreado(resultado, identificador => $"/api/credenciales/{identificador}");
        }

        [HttpPatch("{id:guid}/bloquear")]
        public async Task<IActionResult> Bloquear(Guid id, CancellationToken cancelacion)
        {
            Resultado resultado = await _mediador.Enviar(new BloquearCredencialComando(id), cancelacion);

            return Responder(resultado);
        }

        [HttpPatch("{id:guid}/revocar")]
        public async Task<IActionResult> Revocar(Guid id, CancellationToken cancelacion)
        {
            Resultado resultado = await _mediador.Enviar(new RevocarCredencialComando(id), cancelacion);

            return Responder(resultado);
        }

        [HttpGet("codigo/{codigo}")]
        public async Task<IActionResult> ObtenerPorCodigo(string codigo, CancellationToken cancelacion)
        {
            Resultado<CredencialDto> resultado = await _mediador.Enviar(new ObtenerCredencialPorCodigoConsulta(codigo), cancelacion);

            return Responder(resultado);
        }

        [HttpGet("empleado/{empleadoId:guid}")]
        public async Task<IActionResult> ListarPorEmpleado(Guid empleadoId, CancellationToken cancelacion)
        {
            Resultado<List<CredencialDto>> resultado = await _mediador.Enviar(new ListarCredencialesPorEmpleadoConsulta(empleadoId), cancelacion);

            return Responder(resultado);
        }
    }
}
