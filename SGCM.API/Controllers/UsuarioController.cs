using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SGCM.Application.Dtos.Seguridad_Usuarios;
using SGCM.Application.Interfaces.Seguridad_Usuarios;
using SGCM.Application.Services.Seguridad_Usuarios;
using SGCM.Domain.Exceptions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SGCM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsuarioController : ControllerBase
    {
        public IUsuarioAppService _usuarioAppService { get; }

        public UsuarioController(IUsuarioAppService usuarioAppService)
        {
            _usuarioAppService = usuarioAppService;
        }

        [HttpGet]
        public async Task<ActionResult<UsuarioResponseDto>> GetAll()
        {
            var usuarios = await _usuarioAppService.LeerTodosAsync();
            return Ok(usuarios);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioResponseDto>> GetById(int id)
        {
            var usuario =  await _usuarioAppService.LeerAsync(id);
            return Ok(usuario);
        }

        [HttpGet("rol/{rol}")]
        public async Task<ActionResult<IReadOnlyList<UsuarioResponseDto>>> GetByRol(string rol)
        {
            var usuarios = await _usuarioAppService.LeerPorRolAsync(rol);
            return Ok(usuarios);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<UsuarioResponseDto>> Post([FromBody] CrearUsuarioDto dto)
        {
            var usuario = await _usuarioAppService.CrearAsync(dto);
            return CreatedAtAction(nameof(GetAll), new { id = usuario.Id }, usuario);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UsuarioResponseDto>> Put(int id, [FromBody] ActualizarUsuarioDto dto)
        {
            var usuario = await _usuarioAppService.ActualizarAsync(id, dto);
            return Ok(usuario);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var resultado = await _usuarioAppService.EliminarAsync(id);
            return Ok(resultado);
        }

        [HttpPut("{id}/password")]
        public async Task<ActionResult> ChangePassword(int id, [FromBody] CambiarPasswordUsuarioDto dto)
        {
            await _usuarioAppService.CambiarPasswordAsync(id, dto);
            return Ok();
        }


    }
}
