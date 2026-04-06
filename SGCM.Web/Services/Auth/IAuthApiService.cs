using SGCM.Application.Dtos.Login;
using SGCM.Application.Dtos.Seguridad_Usuarios;

namespace SGCM.Web.Services.Auth
{
    public interface IAuthApiService
    {
        Task<LoginResponseDto?> LoginAsync(LoginDto loginDto);
        Task<LoginResponseDto?> RegisterAsync(RegisterDto registerDto);
        Task<UsuarioResponseDto?> GetCurrentUserAsync(string token);
    }
}
