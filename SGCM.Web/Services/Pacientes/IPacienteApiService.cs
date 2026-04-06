using SGCM.Application.Dtos.Pacientes;

namespace SGCM.Web.Services.Pacientes
{
    public interface IPacienteApiService
    {
        Task<IReadOnlyList<PacienteResponseDto>> GetAllAsync(string token);
        Task<PacienteResponseDto?> GetByIdAsync(string token, int id);
        Task<PacienteResponseDto?> CreateAsync(string token, CrearPacienteDto dto);
        Task<PacienteResponseDto?> UpdateAsync(string token, int id, CrearPacienteDto dto);
        Task<bool> DeleteAsync(string token, int id);
    }
}
