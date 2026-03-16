using Microsoft.AspNetCore.Mvc;
using SGCM.Application.Dtos.Citas_Agenda;
using SGCM.Application.Interfaces.Citas_Agenda;

namespace SGCM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicoController : ControllerBase
    {
        private readonly IMedicoAppService _medicoAppService;

        public MedicoController(IMedicoAppService medicoAppService)
        {
            _medicoAppService = medicoAppService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<MedicoResponseDto>>> GetAll()
        {
            var medicos = await _medicoAppService.LeerTodosAsync();
            return Ok(medicos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MedicoResponseDto>> GetById(int id)
        {
            var medico = await _medicoAppService.LeerAsync(id);
            return Ok(medico);
        }

        [HttpPost]
        public async Task<ActionResult<MedicoResponseDto>> Post([FromBody] CrearMedicoDto dto)
        {
            var medico = await _medicoAppService.CrearAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = medico.Id }, medico);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<MedicoResponseDto>> Put(int id, [FromBody] CrearMedicoDto dto)
        {
            var medico = await _medicoAppService.ActualizarAsync(id, dto);
            return Ok(medico);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var resultado = await _medicoAppService.EliminarAsync(id);
            return Ok(resultado);
        }
    }
}
