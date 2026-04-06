using SGCM.Application.Dtos.Pacientes;

namespace SGCM.Web.Services.Pacientes
{
    public class PacienteApiService : BaseApiService, IPacienteApiService
    {
        public PacienteApiService(HttpClient httpClient, IConfiguration configuration)
            : base(httpClient, configuration)
        {
        }

        public async Task<IReadOnlyList<PacienteResponseDto>> GetAllAsync(string token)
        {
            SetAuthorizationToken(token);
            var result = await GetAsync<IReadOnlyList<PacienteResponseDto>>("api/Paciente");
            ClearAuthorizationToken();
            return result ?? new List<PacienteResponseDto>();
        }

        public async Task<PacienteResponseDto?> GetByIdAsync(string token, int id)
        {
            SetAuthorizationToken(token);
            var result = await GetAsync<PacienteResponseDto>($"api/Paciente/{id}");
            ClearAuthorizationToken();
            return result;
        }

        public async Task<PacienteResponseDto?> CreateAsync(string token, CrearPacienteDto dto)
        {
            SetAuthorizationToken(token);
            var result = await PostAsync<CrearPacienteDto, PacienteResponseDto>("api/Paciente", dto);
            ClearAuthorizationToken();
            return result;
        }

        public async Task<PacienteResponseDto?> UpdateAsync(string token, int id, CrearPacienteDto dto)
        {
            SetAuthorizationToken(token);
            var result = await PutAsync<CrearPacienteDto, PacienteResponseDto>($"api/Paciente/{id}", dto);
            ClearAuthorizationToken();
            return result;
        }

        public async Task<bool> DeleteAsync(string token, int id)
        {
            SetAuthorizationToken(token);
            var result = await DeleteAsync($"api/Paciente/{id}");
            ClearAuthorizationToken();
            return result;
        }
    }
}
