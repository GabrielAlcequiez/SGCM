using SGCM.Domain.Entities.Seguridad_Usuarios;

namespace SGCM.Domain.Repository
{
    public interface IAuditoriaLogsRepository : IBaseRepository<AuditoriaLogs>
    {
        Task<IEnumerable<AuditoriaLogs>> ObtenerPorUsuarioAsync(int usuarioId);
        Task<IEnumerable<AuditoriaLogs>> ObtenerPorRangoFechasAsync(DateTime inicio, DateTime fin);
    }
}
