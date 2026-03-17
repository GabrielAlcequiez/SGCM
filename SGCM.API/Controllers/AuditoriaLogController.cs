using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGCM.Application.Dtos.Seguridad_Usuarios;
using SGCM.Application.Interfaces.Seguridad_Usuarios;

namespace SGCM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class AuditoriaLogController : ControllerBase
    {
        private readonly IAuditoriaLogAppService _auditoriaLogAppService;

        public AuditoriaLogController(IAuditoriaLogAppService auditoriaLogAppService)
        {
            _auditoriaLogAppService = auditoriaLogAppService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<AuditoriaLogResponseDto>>> GetAll()
        {
            var logs = await _auditoriaLogAppService.LeerTodosAsync();
            return Ok(logs);
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<ActionResult<IReadOnlyList<AuditoriaLogResponseDto>>> GetByUsuario(int usuarioId)
        {
            var logs = await _auditoriaLogAppService.LeerPorUsuarioAsync(usuarioId);
            return Ok(logs);
        }

        [HttpGet("rango")]
        public async Task<ActionResult<IReadOnlyList<AuditoriaLogResponseDto>>> GetByRangoFechas([FromQuery] DateTime inicio, [FromQuery] DateTime fin)
        {
            var logs = await _auditoriaLogAppService.LeerPorRangoFechasAsync(inicio, fin);
            return Ok(logs);
        }
    }
}
