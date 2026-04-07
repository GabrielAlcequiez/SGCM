using Microsoft.AspNetCore.Mvc;
using SGCM.Application.Dtos.Pacientes;
using SGCM.Application.Interfaces.Pacientes;

namespace SGCM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize]
    public class PacienteController : ControllerBase
    {
        private readonly IPacienteAppService _pacienteAppService;

        public PacienteController(IPacienteAppService pacienteAppService)
        {
            _pacienteAppService = pacienteAppService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<PacienteResponseDto>>> GetAll()
        {
            var pacientes = await _pacienteAppService.LeerTodosAsync();
            return Ok(pacientes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PacienteResponseDto>> GetById(int id)
        {
            var paciente = await _pacienteAppService.LeerAsync(id);
            return Ok(paciente);
        }

        [HttpPost]
        public async Task<ActionResult<PacienteResponseDto>> Post([FromBody] CrearPacienteDto dto)
        {
            var paciente = await _pacienteAppService.CrearAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = paciente.Id }, paciente);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PacienteResponseDto>> Put(int id, [FromBody] CrearPacienteDto dto)
        {
            var paciente = await _pacienteAppService.ActualizarAsync(id, dto);
            return Ok(paciente);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var resultado = await _pacienteAppService.EliminarAsync(id);
            return Ok(resultado);
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<ActionResult<PacienteResponseDto>> GetByUsuarioId(int usuarioId)
        {
            var paciente = await _pacienteAppService.ObtenerPorUsuarioIdAsync(usuarioId);
            if (paciente == null) return NotFound();
            return Ok(paciente);
        }
    }
}
