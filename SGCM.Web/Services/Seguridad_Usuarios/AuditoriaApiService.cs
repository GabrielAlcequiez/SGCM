using SGCM.Application.Dtos.Seguridad_Usuarios;

namespace SGCM.Web.Services.Seguridad_Usuarios
{
    public class AuditoriaApiService : BaseApiService, IAuditoriaApiService
    {
        public AuditoriaApiService(HttpClient httpClient, IConfiguration configuration)
            : base(httpClient, configuration)
        {
        }

        public async Task<IReadOnlyList<AuditoriaLogResponseDto>> GetAllAsync(string token)
        {
            SetAuthorizationToken(token);
            var result = await GetAsync<IReadOnlyList<AuditoriaLogResponseDto>>("api/auditorialog");
            ClearAuthorizationToken();
            return result ?? new List<AuditoriaLogResponseDto>();
        }

        public async Task<IReadOnlyList<AuditoriaLogResponseDto>> GetByUsuarioAsync(string token, int usuarioId)
        {
            SetAuthorizationToken(token);
            var result = await GetAsync<IReadOnlyList<AuditoriaLogResponseDto>>($"api/auditorialog/usuario/{usuarioId}");
            ClearAuthorizationToken();
            return result ?? new List<AuditoriaLogResponseDto>();
        }

        public async Task<IReadOnlyList<AuditoriaLogResponseDto>> GetByRangoFechasAsync(string token, DateTime inicio, DateTime fin)
        {
            SetAuthorizationToken(token);
            var result = await GetAsync<IReadOnlyList<AuditoriaLogResponseDto>>($"api/auditorialog/rango?inicio={inicio:yyyy-MM-dd}&fin={fin:yyyy-MM-dd}");
            ClearAuthorizationToken();
            return result ?? new List<AuditoriaLogResponseDto>();
        }
    }
}
