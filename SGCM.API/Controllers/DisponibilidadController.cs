using Microsoft.AspNetCore.Mvc;
using SGCM.Application.Dtos.Citas_Agenda;
using SGCM.Application.Interfaces.Citas_Agenda;

namespace SGCM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize]
    public class DisponibilidadController : ControllerBase
    {
        private readonly IDisponibilidadAppService _disponibilidadAppService;

        public DisponibilidadController(IDisponibilidadAppService disponibilidadAppService)
        {
            _disponibilidadAppService = disponibilidadAppService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<DisponibilidadResponseDto>>> GetAll()
        {
            var disponibilidades = await _disponibilidadAppService.LeerTodosAsync();
            return Ok(disponibilidades);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DisponibilidadResponseDto>> GetById(int id)
        {
            var disponibilidad = await _disponibilidadAppService.LeerAsync(id);
            return Ok(disponibilidad);
        }

        [HttpPost]
        public async Task<ActionResult<DisponibilidadResponseDto>> Post([FromBody] CrearDisponibilidadDto dto)
        {
            var disponibilidad = await _disponibilidadAppService.CrearAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = disponibilidad.Id }, disponibilidad);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<DisponibilidadResponseDto>> Put(int id, [FromBody] CrearDisponibilidadDto dto)
        {
            var disponibilidad = await _disponibilidadAppService.ActualizarAsync(id, dto);
            return Ok(disponibilidad);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var resultado = await _disponibilidadAppService.EliminarAsync(id);
            return Ok(resultado);
        }
    }
}
