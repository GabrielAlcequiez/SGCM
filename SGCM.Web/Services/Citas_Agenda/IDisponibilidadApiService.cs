using SGCM.Application.Dtos.Citas_Agenda;

namespace SGCM.Web.Services.Citas_Agenda
{
    public interface IDisponibilidadApiService
    {
        Task<IReadOnlyList<DisponibilidadResponseDto>> GetAllAsync(string token);
        Task<DisponibilidadResponseDto?> GetByIdAsync(string token, int id);
        Task<DisponibilidadResponseDto?> CreateAsync(string token, CrearDisponibilidadDto dto);
        Task<DisponibilidadResponseDto?> UpdateAsync(string token, int id, CrearDisponibilidadDto dto);
        Task<bool> DeleteAsync(string token, int id);
    }
}
