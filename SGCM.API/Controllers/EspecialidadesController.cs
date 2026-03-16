using Microsoft.AspNetCore.Mvc;
using SGCM.Application.Dtos.Citas_Agenda;
using SGCM.Application.Interfaces.Citas_Agenda;

namespace SGCM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EspecialidadesController : ControllerBase
    {
        private readonly IEspecialidadesAppService _especialidadesAppService;

        public EspecialidadesController(IEspecialidadesAppService especialidadesAppService)
        {
            _especialidadesAppService = especialidadesAppService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<EspecialidadesResponseDto>>> GetAll()
        {
            var especialidades = await _especialidadesAppService.LeerTodosAsync();
            return Ok(especialidades);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EspecialidadesResponseDto>> GetById(int id)
        {
            var especialidad = await _especialidadesAppService.LeerAsync(id);
            return Ok(especialidad);
        }

        [HttpPost]
        public async Task<ActionResult<EspecialidadesResponseDto>> Post([FromBody] CrearEspecialidadesDto dto)
        {
            var especialidad = await _especialidadesAppService.CrearAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = especialidad.Id }, especialidad);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EspecialidadesResponseDto>> Put(int id, [FromBody] CrearEspecialidadesDto dto)
        {
            var especialidad = await _especialidadesAppService.ActualizarAsync(id, dto);
            return Ok(especialidad);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var resultado = await _especialidadesAppService.EliminarAsync(id);
            return Ok(resultado);
        }
    }
}
