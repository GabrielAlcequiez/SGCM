using SGCM.Application.Dtos.Citas_Agenda;

namespace SGCM.Web.Services.Citas_Agenda
{
    public class MedicoApiService : BaseApiService, IMedicoApiService
    {
        public MedicoApiService(HttpClient httpClient, IConfiguration configuration)
            : base(httpClient, configuration)
        {
        }

        public async Task<IReadOnlyList<MedicoResponseDto>> GetAllAsync(string token)
        {
            SetAuthorizationToken(token);
            var result = await GetAsync<IReadOnlyList<MedicoResponseDto>>("api/Medico");
            ClearAuthorizationToken();
            return result ?? new List<MedicoResponseDto>();
        }

        public async Task<MedicoResponseDto?> GetByIdAsync(string token, int id)
        {
            SetAuthorizationToken(token);
            var result = await GetAsync<MedicoResponseDto>($"api/Medico/{id}");
            ClearAuthorizationToken();
            return result;
        }

        public async Task<MedicoResponseDto?> CreateAsync(string token, CrearMedicoDto dto)
        {
            SetAuthorizationToken(token);
            var result = await PostAsync<CrearMedicoDto, MedicoResponseDto>("api/Medico", dto);
            ClearAuthorizationToken();
            return result;
        }

        public async Task<MedicoResponseDto?> UpdateAsync(string token, int id, CrearMedicoDto dto)
        {
            SetAuthorizationToken(token);
            var result = await PutAsync<CrearMedicoDto, MedicoResponseDto>($"api/Medico/{id}", dto);
            ClearAuthorizationToken();
            return result;
        }

        public async Task<bool> DeleteAsync(string token, int id)
        {
            SetAuthorizationToken(token);
            var result = await DeleteAsync($"api/Medico/{id}");
            ClearAuthorizationToken();
            return result;
        }
    }
}
