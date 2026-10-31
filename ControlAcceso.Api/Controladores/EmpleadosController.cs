using ControlAcceso.Aplicacion.CasosDeUso.Empleados.Comandos;
using ControlAcceso.Aplicacion.CasosDeUso.Empleados.Consultas;
using ControlAcceso.Aplicacion.CasosDeUso.Empleados.Dtos;
using ControlAcceso.Aplicacion.Utilidades.Mediador;
using ControlAcceso.Aplicacion.Utilidades.Paginacion;
using ControlAcceso.Aplicacion.Utilidades.Resultados;
using ControlAcceso.Dominio.Entidades.Empleados.ObjetosDeValor;
using Microsoft.AspNetCore.Mvc;

namespace ControlAcceso.Api.Controladores
{
    public sealed record ActualizarEmpleadoPeticion(string Nombres,
                                                    string Apellidos,
                                                    string CorreoElectronico,
                                                    string Cargo,
                                                    Guid AreaId);

    [Route("api/empleados")]
    public sealed class EmpleadosController : ControladorApi
    {
        public EmpleadosController(IMediador mediador) : base(mediador)
        {
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearEmpleadoComando comando, CancellationToken cancelacion)
        {
            Resultado<Guid> resultado = await _mediador.Enviar(comando, cancelacion);

            return ResponderCreado(resultado, identificador => $"/api/empleados/{identificador}");
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Actualizar(Guid id,
                                                    [FromBody] ActualizarEmpleadoPeticion peticion,
                                                    CancellationToken cancelacion)
        {
            ActualizarEmpleadoComando comando = new(id,
                                                    peticion.Nombres,
                                                    peticion.Apellidos,
                                                    peticion.CorreoElectronico,
                                                    peticion.Cargo,
                                                    peticion.AreaId);

            Resultado resultado = await _mediador.Enviar(comando, cancelacion);

            return Responder(resultado);
        }

        [HttpPatch("{id:guid}/desactivar")]
        public async Task<IActionResult> Desactivar(Guid id, CancellationToken cancelacion)
        {
            Resultado resultado = await _mediador.Enviar(new DesactivarEmpleadoComando(id), cancelacion);

            return Responder(resultado);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> ObtenerPorId(Guid id, CancellationToken cancelacion)
        {
            Resultado<EmpleadoDto> resultado = await _mediador.Enviar(new ObtenerEmpleadoPorIdConsulta(id), cancelacion);

            return Responder(resultado);
        }

        [HttpGet]
        public async Task<IActionResult> Listar([FromQuery] int numeroPagina,
                                                [FromQuery] int tamanoPagina,
                                                [FromQuery] Guid? areaId,
                                                [FromQuery] EstadoLaboral? estado,
                                                CancellationToken cancelacion)
        {
            ListarEmpleadosConsulta consulta = new(numeroPagina <= 0 ? RespuestaPaginada<EmpleadoDto>.NumeroPaginaPredeterminado : numeroPagina,
                                                   tamanoPagina <= 0 ? RespuestaPaginada<EmpleadoDto>.TamanoPaginaPredeterminado : tamanoPagina,
                                                   areaId,
                                                   estado);

            Resultado<RespuestaPaginada<EmpleadoDto>> resultado = await _mediador.Enviar(consulta, cancelacion);

            return Responder(resultado);
        }
    }
}
