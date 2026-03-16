using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGCM.Application.Dtos.Seguridad_Usuarios;
using SGCM.Application.Interfaces.Seguridad_Usuarios;

namespace SGCM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AdministradorController : ControllerBase
    {
        private readonly IAdministradorAppService _administradorAppService;

        public AdministradorController(IAdministradorAppService administradorAppService)
        {
            _administradorAppService = administradorAppService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<AdministradorResponseDto>>> GetAll()
        {
            var administradores = await _administradorAppService.LeerTodosAsync();
            return Ok(administradores);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AdministradorResponseDto>> GetById(int id)
        {
            var administrador = await _administradorAppService.LeerAsync(id);
            return Ok(administrador);
        }

        [HttpPost]
        public async Task<ActionResult<AdministradorResponseDto>> Post([FromBody] CrearAdministradorDto dto)
        {
            var administrador = await _administradorAppService.CrearAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = administrador.Id }, administrador);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AdministradorResponseDto>> Put(int id, [FromBody] CrearAdministradorDto dto)
        {
            var administrador = await _administradorAppService.ActualizarAsync(id, dto);
            return Ok(administrador);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var resultado = await _administradorAppService.EliminarAsync(id);
            return Ok(resultado);
        }
    }
}
