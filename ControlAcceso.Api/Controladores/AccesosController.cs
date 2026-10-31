using ControlAcceso.Aplicacion.CasosDeUso.RegistrosAcceso.Comandos;
using ControlAcceso.Aplicacion.CasosDeUso.RegistrosAcceso.Consultas;
using ControlAcceso.Aplicacion.CasosDeUso.RegistrosAcceso.Dtos;
using ControlAcceso.Aplicacion.Utilidades.Mediador;
using ControlAcceso.Aplicacion.Utilidades.Paginacion;
using ControlAcceso.Aplicacion.Utilidades.Resultados;
using ControlAcceso.Dominio.Entidades.RegistrosAcceso.ObjetosDeValor;
using Microsoft.AspNetCore.Mvc;

namespace ControlAcceso.Api.Controladores
{
    [Route("api/accesos")]
    public sealed class AccesosController : ControladorApi
    {
        public AccesosController(IMediador mediador) : base(mediador)
        {
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarMovimiento([FromBody] RegistrarMovimientoComando comando, CancellationToken cancelacion)
        {
            Resultado<ResultadoMovimientoDto> resultado = await _mediador.Enviar(comando, cancelacion);

            return Responder(resultado);
        }

        [HttpGet]
        public async Task<IActionResult> ConsultarHistorial([FromQuery] int numeroPagina,
                                                            [FromQuery] int tamanoPagina,
                                                            [FromQuery] Guid? empleadoId,
                                                            [FromQuery] DateTime? desde,
                                                            [FromQuery] DateTime? hasta,
                                                            [FromQuery] Guid? puntoAccesoId,
                                                            [FromQuery] ResultadoAcceso? resultado,
                                                            CancellationToken cancelacion)
        {
            ConsultarHistorialAccesosConsulta consulta = new(numeroPagina <= 0 ? RespuestaPaginada<RegistroAccesoDto>.NumeroPaginaPredeterminado : numeroPagina,
                                                             tamanoPagina <= 0 ? RespuestaPaginada<RegistroAccesoDto>.TamanoPaginaPredeterminado : tamanoPagina,
                                                             empleadoId,
                                                             desde,
                                                             hasta,
                                                             puntoAccesoId,
                                                             resultado);

            Resultado<RespuestaPaginada<RegistroAccesoDto>> resultadoConsulta = await _mediador.Enviar(consulta, cancelacion);

            return Responder(resultadoConsulta);
        }
    }
}
