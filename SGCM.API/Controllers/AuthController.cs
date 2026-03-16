using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGCM.Application.Dtos.Login;
using SGCM.Application.Dtos.Seguridad_Usuarios;
using SGCM.Application.Interfaces;
using SGCM.Application.Interfaces.Seguridad_Usuarios;
using SGCM.Domain.Exceptions;

namespace SGCM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAutenticacionService _autenticacionService;
        private readonly IUsuarioAppService _usuarioAppService;
        private readonly ITokenService _tokenService;

        public AuthController(IAutenticacionService autenticacionService, IUsuarioAppService usuarioAppService, ITokenService tokenService)
        {
            _autenticacionService = autenticacionService;
            _usuarioAppService = usuarioAppService;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginDto dto)
        {
            var resultado = await _autenticacionService.LoginAsync(dto);
            return Ok(resultado);
        }

        [HttpPost("register")]
        public async Task<ActionResult<LoginResponseDto>> Register([FromBody] RegisterDto dto)
        {
            var resultado = await _autenticacionService.RegisterAsync(dto);
            return CreatedAtAction(nameof(Me), new { id = resultado.UsuarioId }, resultado);
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<UsuarioResponseDto>> Me()
        {
            var usuarioId = _tokenService.ObtenerUsuarioIdActual();
            var usuario = await _usuarioAppService.LeerAsync(usuarioId);
            return Ok(usuario);
        }
    }
}
