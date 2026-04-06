using SGCM.Application.Dtos.Citas_Agenda;

namespace SGCM.Web.Services.Citas_Agenda
{
    public interface IEspecialidadesApiService
    {
        Task<IReadOnlyList<EspecialidadesResponseDto>> GetAllAsync(string token);
        Task<EspecialidadesResponseDto?> GetByIdAsync(string token, int id);
        Task<EspecialidadesResponseDto?> CreateAsync(string token, CrearEspecialidadesDto dto);
        Task<EspecialidadesResponseDto?> UpdateAsync(string token, int id, CrearEspecialidadesDto dto);
        Task<bool> DeleteAsync(string token, int id);
    }
}
