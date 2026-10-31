using ControlAcceso.Aplicacion.CasosDeUso.PuntosAcceso.Comandos;
using ControlAcceso.Aplicacion.CasosDeUso.PuntosAcceso.Consultas;
using ControlAcceso.Aplicacion.CasosDeUso.PuntosAcceso.Dtos;
using ControlAcceso.Aplicacion.Utilidades.Mediador;
using ControlAcceso.Aplicacion.Utilidades.Paginacion;
using ControlAcceso.Aplicacion.Utilidades.Resultados;
using ControlAcceso.Dominio.Entidades.PuntosAcceso.ObjetosDeValor;
using Microsoft.AspNetCore.Mvc;

namespace ControlAcceso.Api.Controladores
{
    [Route("api/puntos-acceso")]
    public sealed class PuntosAccesoController : ControladorApi
    {
        public PuntosAccesoController(IMediador mediador) : base(mediador)
        {
        }

        [HttpPost]
        public async Task<IActionResult> Registrar([FromBody] RegistrarPuntoAccesoComando comando, CancellationToken cancelacion)
        {
            Resultado<Guid> resultado = await _mediador.Enviar(comando, cancelacion);

            return ResponderCreado(resultado, identificador => $"/api/puntos-acceso/{identificador}");
        }

        [HttpPatch("{id:guid}/habilitar")]
        public async Task<IActionResult> Habilitar(Guid id, CancellationToken cancelacion)
        {
            Resultado resultado = await _mediador.Enviar(new CambiarEstadoPuntoAccesoComando(id, true), cancelacion);

            return Responder(resultado);
        }

        [HttpPatch("{id:guid}/deshabilitar")]
        public async Task<IActionResult> Deshabilitar(Guid id, CancellationToken cancelacion)
        {
            Resultado resultado = await _mediador.Enviar(new CambiarEstadoPuntoAccesoComando(id, false), cancelacion);

            return Responder(resultado);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> ObtenerPorId(Guid id, CancellationToken cancelacion)
        {
            Resultado<PuntoAccesoDto> resultado = await _mediador.Enviar(new ObtenerPuntoAccesoPorIdConsulta(id), cancelacion);

            return Responder(resultado);
        }

        [HttpGet]
        public async Task<IActionResult> Listar([FromQuery] int numeroPagina,
                                                [FromQuery] int tamanoPagina,
                                                [FromQuery] bool? habilitado,
                                                [FromQuery] TipoPuntoAcceso? tipo,
                                                CancellationToken cancelacion)
        {
            ListarPuntosAccesoConsulta consulta = new(numeroPagina <= 0 ? RespuestaPaginada<PuntoAccesoDto>.NumeroPaginaPredeterminado : numeroPagina,
                                                      tamanoPagina <= 0 ? RespuestaPaginada<PuntoAccesoDto>.TamanoPaginaPredeterminado : tamanoPagina,
                                                      habilitado,
                                                      tipo);

            Resultado<RespuestaPaginada<PuntoAccesoDto>> resultado = await _mediador.Enviar(consulta, cancelacion);

            return Responder(resultado);
        }
    }
}
