using SGCM.Application.Dtos.Citas_Agenda;

namespace SGCM.Web.Services.Citas_Agenda
{
    public interface IMedicoApiService
    {
        Task<IReadOnlyList<MedicoResponseDto>> GetAllAsync(string token);
        Task<MedicoResponseDto?> GetByIdAsync(string token, int id);
        Task<MedicoResponseDto?> CreateAsync(string token, CrearMedicoDto dto);
        Task<MedicoResponseDto?> UpdateAsync(string token, int id, CrearMedicoDto dto);
        Task<bool> DeleteAsync(string token, int id);
    }
}
