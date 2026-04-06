using SGCM.Application.Dtos.Pacientes;

namespace SGCM.Web.Services.Pacientes
{
    public interface IProveedoresApiService
    {
        Task<IReadOnlyList<ProveedoresResponseDto>> GetAllAsync(string token);
        Task<ProveedoresResponseDto?> GetByIdAsync(string token, int id);
        Task<ProveedoresResponseDto?> CreateAsync(string token, CrearProveedoresDto dto);
        Task<ProveedoresResponseDto?> UpdateAsync(string token, int id, CrearProveedoresDto dto);
        Task<bool> DeleteAsync(string token, int id);
    }
}
