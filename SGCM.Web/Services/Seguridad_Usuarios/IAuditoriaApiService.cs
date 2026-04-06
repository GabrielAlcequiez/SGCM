using SGCM.Application.Dtos.Seguridad_Usuarios;

namespace SGCM.Web.Services.Seguridad_Usuarios
{
    public interface IAuditoriaApiService
    {
        Task<IReadOnlyList<AuditoriaLogResponseDto>> GetAllAsync(string token);
        Task<IReadOnlyList<AuditoriaLogResponseDto>> GetByUsuarioAsync(string token, int usuarioId);
        Task<IReadOnlyList<AuditoriaLogResponseDto>> GetByRangoFechasAsync(string token, DateTime inicio, DateTime fin);
    }
}
