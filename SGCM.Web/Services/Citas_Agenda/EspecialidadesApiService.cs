using SGCM.Application.Dtos.Citas_Agenda;

namespace SGCM.Web.Services.Citas_Agenda
{
    public class EspecialidadesApiService : BaseApiService, IEspecialidadesApiService
    {
        public EspecialidadesApiService(HttpClient httpClient, IConfiguration configuration)
            : base(httpClient, configuration)
        {
        }

        public async Task<IReadOnlyList<EspecialidadesResponseDto>> GetAllAsync(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                SetAuthorizationToken(token);
            }
            var result = await GetAsync<IReadOnlyList<EspecialidadesResponseDto>>("api/Especialidades");
            if (!string.IsNullOrEmpty(token))
            {
                ClearAuthorizationToken();
            }
            return result ?? new List<EspecialidadesResponseDto>();
        }

        public async Task<EspecialidadesResponseDto?> GetByIdAsync(string token, int id)
        {
            SetAuthorizationToken(token);
            var result = await GetAsync<EspecialidadesResponseDto>($"api/Especialidades/{id}");
            ClearAuthorizationToken();
            return result;
        }

        public async Task<EspecialidadesResponseDto?> CreateAsync(string token, CrearEspecialidadesDto dto)
        {
            SetAuthorizationToken(token);
            var result = await PostAsync<CrearEspecialidadesDto, EspecialidadesResponseDto>("api/Especialidades", dto);
            ClearAuthorizationToken();
            return result;
        }

        public async Task<EspecialidadesResponseDto?> UpdateAsync(string token, int id, CrearEspecialidadesDto dto)
        {
            SetAuthorizationToken(token);
            var result = await PutAsync<CrearEspecialidadesDto, EspecialidadesResponseDto>($"api/Especialidades/{id}", dto);
            ClearAuthorizationToken();
            return result;
        }

        public async Task<bool> DeleteAsync(string token, int id)
        {
            SetAuthorizationToken(token);
            var result = await DeleteAsync($"api/Especialidades/{id}");
            ClearAuthorizationToken();
            return result;
        }
    }
}
