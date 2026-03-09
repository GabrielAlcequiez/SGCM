using SGCM.Application.Dtos.Seguridad_Usuarios;

namespace SGCM.Application.Interfaces
{
    public interface IAuditoriaLogAppService
    {
        Task<IReadOnlyList<AuditoriaLogResponseDto>> LeerTodosAsync();
        Task<IReadOnlyList<AuditoriaLogResponseDto>> LeerPorUsuarioAsync(int usuarioId);
        Task<IReadOnlyList<AuditoriaLogResponseDto>> LeerPorRangoFechasAsync(DateTime inicio, DateTime fin);
    }
}
