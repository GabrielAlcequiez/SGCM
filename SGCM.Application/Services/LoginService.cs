using SGCM.Application.Dtos.Login;
using SGCM.Application.Interfaces;

namespace SGCM.Application.Services
{
    public class LoginService : IAutenticacionService
    {
        public Task<LoginResponseDto> LoginAsync(LoginDto request)
        {
            throw new NotImplementedException();
        }
    }
}
