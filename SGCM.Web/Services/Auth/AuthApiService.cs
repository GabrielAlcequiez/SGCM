using SGCM.Application.Dtos.Login;
using SGCM.Application.Dtos.Seguridad_Usuarios;

namespace SGCM.Web.Services.Auth
{
    public class AuthApiService : BaseApiService, IAuthApiService
    {
        public AuthApiService(HttpClient httpClient, IConfiguration configuration) 
            : base(httpClient, configuration)
        {
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginDto loginDto)
        {
            var result = await PostAsync<LoginDto, LoginResponseDto>("api/auth/login", loginDto);
            return result;
        }

        public async Task<LoginResponseDto?> RegisterAsync(RegisterDto registerDto)
        {
            var response = await PostAsyncWithResponse("api/auth/register", registerDto);
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<LoginResponseDto>(_jsonOptions);
            }
            
            return null;
        }

        public async Task<UsuarioResponseDto?> GetCurrentUserAsync(string token)
        {
            SetAuthorizationToken(token);
            var result = await GetAsync<UsuarioResponseDto>("api/auth/me");
            ClearAuthorizationToken();
            return result;
        }
    }
}
