using SGCM.Application.Dtos.Pacientes;

namespace SGCM.Web.Services.Pacientes
{
    public class ProveedoresApiService : BaseApiService, IProveedoresApiService
    {
        public ProveedoresApiService(HttpClient httpClient, IConfiguration configuration)
            : base(httpClient, configuration)
        {
        }

        public async Task<IReadOnlyList<ProveedoresResponseDto>> GetAllAsync(string token)
        {
            SetAuthorizationToken(token);
            var result = await GetAsync<IReadOnlyList<ProveedoresResponseDto>>("api/Proveedores");
            ClearAuthorizationToken();
            return result ?? new List<ProveedoresResponseDto>();
        }

        public async Task<ProveedoresResponseDto?> GetByIdAsync(string token, int id)
        {
            SetAuthorizationToken(token);
            var result = await GetAsync<ProveedoresResponseDto>($"api/Proveedores/{id}");
            ClearAuthorizationToken();
            return result;
        }

        public async Task<ProveedoresResponseDto?> CreateAsync(string token, CrearProveedoresDto dto)
        {
            SetAuthorizationToken(token);
            var result = await PostAsync<CrearProveedoresDto, ProveedoresResponseDto>("api/Proveedores", dto);
            ClearAuthorizationToken();
            return result;
        }

        public async Task<ProveedoresResponseDto?> UpdateAsync(string token, int id, CrearProveedoresDto dto)
        {
            SetAuthorizationToken(token);
            var result = await PutAsync<CrearProveedoresDto, ProveedoresResponseDto>($"api/Proveedores/{id}", dto);
            ClearAuthorizationToken();
            return result;
        }

        public async Task<bool> DeleteAsync(string token, int id)
        {
            SetAuthorizationToken(token);
            var result = await DeleteAsync($"api/Proveedores/{id}");
            ClearAuthorizationToken();
            return result;
        }
    }
}
