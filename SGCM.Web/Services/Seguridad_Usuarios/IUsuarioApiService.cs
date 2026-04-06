using SGCM.Application.Dtos.Seguridad_Usuarios;

namespace SGCM.Web.Services.Seguridad_Usuarios
{
    public interface IUsuarioApiService
    {
        Task<IReadOnlyList<UsuarioResponseDto>> GetAllAsync(string token);
        Task<UsuarioResponseDto?> GetByIdAsync(string token, int id);
        Task<IReadOnlyList<UsuarioResponseDto>> GetByRolAsync(string token, string rol);
        Task<UsuarioResponseDto?> CreateAsync(string token, CrearUsuarioDto dto);
        Task<UsuarioResponseDto?> UpdateAsync(string token, int id, ActualizarUsuarioDto dto);
        Task<bool> DeleteAsync(string token, int id);
        Task ChangePasswordAsync(string token, int id, CambiarPasswordUsuarioDto dto);
    }
}
