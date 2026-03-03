using Microsoft.AspNetCore.Mvc;
using SGCM.Domain.Entities.Pacientes;
using SGCM.Domain.Repository;

namespace SGCM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PacienteController : ControllerBase
    {
        private readonly IPacienteRepository _pacienteRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PacienteController(IPacienteRepository pacienteRepository, IUnitOfWork unitOfWork)
        {
            _pacienteRepository = pacienteRepository;
            _unitOfWork = unitOfWork;
        }

        // GET: api/paciente
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var pacientes = await _pacienteRepository.ObtenerTodosAsync();
            return Ok(pacientes);
        }

        // GET: api/paciente/1
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var paciente = await _pacienteRepository.ObtenerPorIdAsync(id);
            if (paciente is null)
                return NotFound($"Paciente con ID {id} no encontrado.");
            return Ok(paciente);
        }

        // POST: api/paciente
        [HttpPost]
        public async Task<IActionResult> Agregar([FromBody] CrearPacienteRequest request)
        {
            var paciente = new Paciente(
                request.Nombre,
                request.Apellido,
                request.Telefono,
                request.Direccion,
                request.FechaNacimiento,
                request.ProveedorId,
                request.NSS,
                request.UsuarioId
            );

            await _pacienteRepository.AgregarAsync(paciente);
            await _unitOfWork.CommitAsync();

            return Ok("Paciente creado correctamente.");
        }

        // DELETE: api/paciente/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            await _pacienteRepository.EliminarAsync(id);
            await _unitOfWork.CommitAsync();
            return Ok($"Paciente con ID {id} eliminado.");
        }
    }

    // DTO de entrada para crear paciente
    public record CrearPacienteRequest(
        string Nombre,
        string Apellido,
        string Telefono,
        string Direccion,
        DateOnly FechaNacimiento,
        int? ProveedorId,
        string? NSS,
        int UsuarioId
    );
}