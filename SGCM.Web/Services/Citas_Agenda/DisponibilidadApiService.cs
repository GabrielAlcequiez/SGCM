using SGCM.Application.Dtos.Citas_Agenda;

namespace SGCM.Web.Services.Citas_Agenda
{
    public class DisponibilidadApiService : BaseApiService, IDisponibilidadApiService
    {
        public DisponibilidadApiService(HttpClient httpClient, IConfiguration configuration)
            : base(httpClient, configuration)
        {
        }

        public async Task<IReadOnlyList<DisponibilidadResponseDto>> GetAllAsync(string token)
        {
            SetAuthorizationToken(token);
            var result = await GetAsync<IReadOnlyList<DisponibilidadResponseDto>>("api/Disponibilidad");
            ClearAuthorizationToken();
            return result ?? new List<DisponibilidadResponseDto>();
        }

        public async Task<DisponibilidadResponseDto?> GetByIdAsync(string token, int id)
        {
            SetAuthorizationToken(token);
            var result = await GetAsync<DisponibilidadResponseDto>($"api/Disponibilidad/{id}");
            ClearAuthorizationToken();
            return result;
        }

        public async Task<DisponibilidadResponseDto?> CreateAsync(string token, CrearDisponibilidadDto dto)
        {
            SetAuthorizationToken(token);
            var result = await PostAsync<CrearDisponibilidadDto, DisponibilidadResponseDto>("api/Disponibilidad", dto);
            ClearAuthorizationToken();
            return result;
        }

        public async Task<DisponibilidadResponseDto?> UpdateAsync(string token, int id, CrearDisponibilidadDto dto)
        {
            SetAuthorizationToken(token);
            var result = await PutAsync<CrearDisponibilidadDto, DisponibilidadResponseDto>($"api/Disponibilidad/{id}", dto);
            ClearAuthorizationToken();
            return result;
        }

        public async Task<bool> DeleteAsync(string token, int id)
        {
            SetAuthorizationToken(token);
            var result = await DeleteAsync($"api/Disponibilidad/{id}");
            ClearAuthorizationToken();
            return result;
        }
    }
}
