using Microsoft.AspNetCore.Mvc;
using SGCM.Application.Dtos.Pacientes;
using SGCM.Application.Interfaces.Pacientes;

namespace SGCM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize]
    public class ProveedoresController : ControllerBase
    {
        private readonly IProveedoresAppService _proveedoresAppService;

        public ProveedoresController(IProveedoresAppService proveedoresAppService)
        {
            _proveedoresAppService = proveedoresAppService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProveedoresResponseDto>>> GetAll()
        {
            var proveedores = await _proveedoresAppService.LeerTodosAsync();
            return Ok(proveedores);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProveedoresResponseDto>> GetById(int id)
        {
            var proveedor = await _proveedoresAppService.LeerAsync(id);
            return Ok(proveedor);
        }

        [HttpPost]
        public async Task<ActionResult<ProveedoresResponseDto>> Post([FromBody] CrearProveedoresDto dto)
        {
            var proveedor = await _proveedoresAppService.CrearAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = proveedor.Id }, proveedor);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProveedoresResponseDto>> Put(int id, [FromBody] CrearProveedoresDto dto)
        {
            var proveedor = await _proveedoresAppService.ActualizarAsync(id, dto);
            return Ok(proveedor);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var resultado = await _proveedoresAppService.EliminarAsync(id);
            return Ok(resultado);
        }
    }
}
