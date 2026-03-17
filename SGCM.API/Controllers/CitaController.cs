using Microsoft.AspNetCore.Mvc;
using SGCM.Application.Dtos.Citas_Agenda;
using SGCM.Application.Interfaces.Citas_Agenda;

namespace SGCM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize]
    public class CitaController : ControllerBase
    {
        private readonly ICitasAppService _citasAppService;

        public CitaController(ICitasAppService citasAppService)
        {
            _citasAppService = citasAppService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<CitaResponseDto>>> GetAll()
        {
            var citas = await _citasAppService.LeerTodosAsync();
            return Ok(citas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CitaResponseDto>> GetById(int id)
        {
            var cita = await _citasAppService.LeerAsync(id);
            return Ok(cita);
        }

        [HttpPost]
        public async Task<ActionResult<CitaResponseDto>> Post([FromBody] CrearCitaDto dto)
        {
            var cita = await _citasAppService.CrearAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = cita.Id }, cita);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CitaResponseDto>> Put(int id, [FromBody] CrearCitaDto dto)
        {
            var cita = await _citasAppService.ActualizarAsync(id, dto);
            return Ok(cita);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var resultado = await _citasAppService.EliminarAsync(id);
            return Ok(resultado);
        }

        [HttpPut("{id}/cancelar")]
        public async Task<ActionResult<bool>> Cancelar(int id)
        {
            var resultado = await _citasAppService.CancelarCitaAsync(id);
            return Ok(resultado);
        }

        [HttpPut("{id}/completar")]
        public async Task<ActionResult<bool>> Completar(int id)
        {
            var resultado = await _citasAppService.CompletarCitaAsync(id);
            return Ok(resultado);
        }
    }
}
