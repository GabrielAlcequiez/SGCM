using SGCM.Application.Dtos.Seguridad_Usuarios;

namespace SGCM.Web.Services.Seguridad_Usuarios
{
    public class UsuarioApiService : BaseApiService, IUsuarioApiService
    {
        public UsuarioApiService(HttpClient httpClient, IConfiguration configuration)
            : base(httpClient, configuration)
        {
        }

        public async Task<IReadOnlyList<UsuarioResponseDto>> GetAllAsync(string token)
        {
            SetAuthorizationToken(token);
            var result = await GetAsync<IReadOnlyList<UsuarioResponseDto>>("api/usuario");
            ClearAuthorizationToken();
            return result ?? new List<UsuarioResponseDto>();
        }

        public async Task<UsuarioResponseDto?> GetByIdAsync(string token, int id)
        {
            SetAuthorizationToken(token);
            var result = await GetAsync<UsuarioResponseDto>($"api/usuario/{id}");
            ClearAuthorizationToken();
            return result;
        }

        public async Task<IReadOnlyList<UsuarioResponseDto>> GetByRolAsync(string token, string rol)
        {
            SetAuthorizationToken(token);
            var result = await GetAsync<IReadOnlyList<UsuarioResponseDto>>($"api/usuario/rol/{rol}");
            ClearAuthorizationToken();
            return result ?? new List<UsuarioResponseDto>();
        }

        public async Task<UsuarioResponseDto?> CreateAsync(string token, CrearUsuarioDto dto)
        {
            SetAuthorizationToken(token);
            var result = await PostAsync<CrearUsuarioDto, UsuarioResponseDto>("api/usuario", dto);
            ClearAuthorizationToken();
            return result;
        }

        public async Task<UsuarioResponseDto?> UpdateAsync(string token, int id, ActualizarUsuarioDto dto)
        {
            SetAuthorizationToken(token);
            var result = await PutAsync<ActualizarUsuarioDto, UsuarioResponseDto>($"api/usuario/{id}", dto);
            ClearAuthorizationToken();
            return result;
        }

        public async Task<bool> DeleteAsync(string token, int id)
        {
            SetAuthorizationToken(token);
            var result = await DeleteAsync($"api/usuario/{id}");
            ClearAuthorizationToken();
            return result;
        }

        public async Task ChangePasswordAsync(string token, int id, CambiarPasswordUsuarioDto dto)
        {
            SetAuthorizationToken(token);
            await _httpClient.PutAsJsonAsync($"api/usuario/{id}/password", dto, _jsonOptions);
            ClearAuthorizationToken();
        }
    }
}
