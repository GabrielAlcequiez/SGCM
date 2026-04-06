using SGCM.Application.Dtos.Citas_Agenda;

namespace SGCM.Web.Services.Citas_Agenda
{
    public interface ICitasApiService
    {
        Task<IReadOnlyList<CitaResponseDto>> GetAllAsync(string token);
        Task<CitaResponseDto?> GetByIdAsync(string token, int id);
        Task<CitaResponseDto?> CreateAsync(string token, CrearCitaDto dto);
        Task<CitaResponseDto?> UpdateAsync(string token, int id, CrearCitaDto dto);
        Task<bool> DeleteAsync(string token, int id);
        Task<bool> CancelarAsync(string token, int id);
        Task<bool> CompletarAsync(string token, int id);
    }
}
