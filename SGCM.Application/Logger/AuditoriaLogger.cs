using SGCM.Domain.Entities.Seguridad_Usuarios;
using SGCM.Domain.Repository;

namespace SGCM.Application.Logger
{
    public class AuditoriaLogger : IAuditoriaLogger
    {
        private readonly IAuditoriaLogsRepository _logsRepository;

        public AuditoriaLogger(IAuditoriaLogsRepository repository)
        {
            _logsRepository = repository;
        }

        public async Task RegistrarAsync(int usuarioId, string accion, string entidadAfectada, string detalles)
        {
            var audi = new AuditoriaLogs(usuarioId, accion, entidadAfectada, detalles);
            await _logsRepository.AgregarAsync(audi);
        }
    }
}
