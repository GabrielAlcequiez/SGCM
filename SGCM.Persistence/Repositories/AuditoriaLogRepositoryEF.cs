using Microsoft.EntityFrameworkCore;
using SGCM.Domain.Entities.Seguridad_Usuarios;
using SGCM.Domain.Exceptions;
using SGCM.Domain.Repository;
using SGCM.Persistence.Context;

namespace SGCM.Persistence.Repositories
{
    public sealed class AuditoriaLogRepositoryEF : IAuditoriaLogsRepository
    {

        private readonly SGCMContext _context;
        public AuditoriaLogRepositoryEF(IUnitOfWork unitOfWork, SGCMContext context) =>
            _context = context;
        
       
        #region Metodos de Registro

        public Task ActualizarAsync(AuditoriaLogs entidad)
        {
            _context.AuditoriaLogs.Update(entidad);
            return Task.CompletedTask;
        }
        public Task AgregarAsync(AuditoriaLogs entidad)
        {
            _context.AuditoriaLogs.Add(entidad);
            return Task.CompletedTask;
        }
        public async Task EliminarAsync(int id)
        {
            var log = await ObtenerPorIdAsync(id);
            if (log is null)
                throw new ExcepcionNoEncontrado("Auditoria_Logs", id);
            _context.AuditoriaLogs.Remove(log);
        }

        #endregion

        #region Metodos de Consulta

        public async Task<AuditoriaLogs?> ObtenerPorIdAsync(int id) =>
            await _context.AuditoriaLogs.FindAsync(id);

        public async Task<IEnumerable<AuditoriaLogs>> ObtenerTodosAsync() =>
            await _context.AuditoriaLogs.ToListAsync();

        public async Task<IEnumerable<AuditoriaLogs>> ObtenerPorUsuarioAsync(int usuarioId) =>
            await _context.AuditoriaLogs.Where(a => a.UsuarioId == usuarioId).ToListAsync();

        public async Task<IEnumerable<AuditoriaLogs>> ObtenerPorRangoFechasAsync(DateTime inicio, DateTime fin) =>
            await _context.AuditoriaLogs.Where(a => a.Fecha >= inicio && a.Fecha <= fin).ToListAsync();

        #endregion
    }
}
