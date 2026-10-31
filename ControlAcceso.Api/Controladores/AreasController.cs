using ControlAcceso.Aplicacion.CasosDeUso.Areas.Consultas;
using ControlAcceso.Aplicacion.CasosDeUso.Areas.Dtos;
using ControlAcceso.Aplicacion.Utilidades.Mediador;
using ControlAcceso.Aplicacion.Utilidades.Resultados;
using Microsoft.AspNetCore.Mvc;

namespace ControlAcceso.Api.Controladores
{
    [Route("api/areas")]
    public sealed class AreasController : ControladorApi
    {
        public AreasController(IMediador mediador) : base(mediador)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Listar(CancellationToken cancelacion)
        {
            Resultado<List<AreaDto>> resultado = await _mediador.Enviar(new ListarAreasConsulta(), cancelacion);

            return Responder(resultado);
        }
    }
}
