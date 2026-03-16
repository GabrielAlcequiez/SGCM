using SGCM.Application.Dtos.Login;

namespace SGCM.Application.Interfaces
{
    public interface IAutenticacionService
    {
        Task<LoginResponseDto> LoginAsync(LoginDto request);
        Task<LoginResponseDto> RegisterAsync(RegisterDto request);
    }
}
