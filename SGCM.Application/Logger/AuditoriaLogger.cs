using SGCM.Domain.Entities.Seguridad_Usuarios;
using SGCM.Domain.Repository;

namespace SGCM.Application.Logger
{
    public class AuditoriaLogger : IAuditoriaLogger
    {
        private readonly IAuditoriaLogsRepository _logsRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AuditoriaLogger(IAuditoriaLogsRepository repository, IUnitOfWork unitOfWork)
        {
            _logsRepository = repository;
            _unitOfWork = unitOfWork;
        }
        public async Task RegistrarAsync(int usuarioId, string accion, string entidadAfectada, string detalles)
        {
            var audi = new AuditoriaLogs(usuarioId, accion, entidadAfectada, detalles);

            await _logsRepository.AgregarAsync(audi);
            await _unitOfWork.CommitAsync();
        }
    }
}
