using SGCM.Application.Dtos.Seguridad_Usuarios;

namespace SGCM.Web.Services.Seguridad_Usuarios
{
    public class AdministradorApiService : BaseApiService, IAdministradorApiService
    {
        public AdministradorApiService(HttpClient httpClient, IConfiguration configuration)
            : base(httpClient, configuration)
        {
        }

        public async Task<IReadOnlyList<AdministradorResponseDto>> GetAllAsync(string token)
        {
            SetAuthorizationToken(token);
            var result = await GetAsync<IReadOnlyList<AdministradorResponseDto>>("api/administrador");
            ClearAuthorizationToken();
            return result ?? new List<AdministradorResponseDto>();
        }

        public async Task<AdministradorResponseDto?> GetByIdAsync(string token, int id)
        {
            SetAuthorizationToken(token);
            var result = await GetAsync<AdministradorResponseDto>($"api/administrador/{id}");
            ClearAuthorizationToken();
            return result;
        }

        public async Task<AdministradorResponseDto?> CreateAsync(string token, CrearAdministradorDto dto)
        {
            SetAuthorizationToken(token);
            var result = await PostAsync<CrearAdministradorDto, AdministradorResponseDto>("api/administrador", dto);
            ClearAuthorizationToken();
            return result;
        }

        public async Task<AdministradorResponseDto?> UpdateAsync(string token, int id, CrearAdministradorDto dto)
        {
            SetAuthorizationToken(token);
            var result = await PutAsync<CrearAdministradorDto, AdministradorResponseDto>($"api/administrador/{id}", dto);
            ClearAuthorizationToken();
            return result;
        }

        public async Task<bool> DeleteAsync(string token, int id)
        {
            SetAuthorizationToken(token);
            var result = await DeleteAsync($"api/administrador/{id}");
            ClearAuthorizationToken();
            return result;
        }
    }
}
