using SGCM.Application.Dtos.Seguridad_Usuarios;

namespace SGCM.Web.Services.Seguridad_Usuarios
{
    public interface IAdministradorApiService
    {
        Task<IReadOnlyList<AdministradorResponseDto>> GetAllAsync(string token);
        Task<AdministradorResponseDto?> GetByIdAsync(string token, int id);
        Task<AdministradorResponseDto?> CreateAsync(string token, CrearAdministradorDto dto);
        Task<AdministradorResponseDto?> UpdateAsync(string token, int id, CrearAdministradorDto dto);
        Task<bool> DeleteAsync(string token, int id);
    }
}
