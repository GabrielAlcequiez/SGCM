using SGCM.Application.Dtos.Citas_Agenda;

namespace SGCM.Web.Services.Citas_Agenda
{
    public class CitasApiService : BaseApiService, ICitasApiService
    {
        public CitasApiService(HttpClient httpClient, IConfiguration configuration)
            : base(httpClient, configuration)
        {
        }

        public async Task<IReadOnlyList<CitaResponseDto>> GetAllAsync(string token)
        {
            SetAuthorizationToken(token);
            var result = await GetAsync<IReadOnlyList<CitaResponseDto>>("api/Cita");
            ClearAuthorizationToken();
            return result ?? new List<CitaResponseDto>();
        }

        public async Task<CitaResponseDto?> GetByIdAsync(string token, int id)
        {
            SetAuthorizationToken(token);
            var result = await GetAsync<CitaResponseDto>($"api/Cita/{id}");
            ClearAuthorizationToken();
            return result;
        }

        public async Task<CitaResponseDto?> CreateAsync(string token, CrearCitaDto dto)
        {
            SetAuthorizationToken(token);
            var result = await PostAsync<CrearCitaDto, CitaResponseDto>("api/Cita", dto);
            ClearAuthorizationToken();
            return result;
        }

        public async Task<CitaResponseDto?> UpdateAsync(string token, int id, CrearCitaDto dto)
        {
            SetAuthorizationToken(token);
            var result = await PutAsync<CrearCitaDto, CitaResponseDto>($"api/Cita/{id}", dto);
            ClearAuthorizationToken();
            return result;
        }

        public async Task<bool> DeleteAsync(string token, int id)
        {
            SetAuthorizationToken(token);
            var result = await DeleteAsync($"api/Cita/{id}");
            ClearAuthorizationToken();
            return result;
        }

        public async Task<bool> CancelarAsync(string token, int id)
        {
            SetAuthorizationToken(token);
            var response = await _httpClient.PutAsync($"api/Cita/{id}/cancelar", null);
            ClearAuthorizationToken();
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> CompletarAsync(string token, int id)
        {
            SetAuthorizationToken(token);
            var response = await _httpClient.PutAsync($"api/Cita/{id}/completar", null);
            ClearAuthorizationToken();
            return response.IsSuccessStatusCode;
        }

        public async Task<FranjasDisponiblesResponseDto?> GetFranjasDisponiblesAsync(string token, int medicoId)
        {
            SetAuthorizationToken(token);
            var result = await GetAsync<FranjasDisponiblesResponseDto>($"api/Cita/franjas/{medicoId}");
            ClearAuthorizationToken();
            return result;
        }
    }
}
