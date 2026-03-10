using SGCM.Application.Dtos.Seguridad_Usuarios;
using SGCM.Application.Interfaces.Seguridad_Usuarios;
using SGCM.Domain.Exceptions;
using SGCM.Domain.Repository;

namespace SGCM.Application.Services.Seguridad_Usuarios
{
    public class AuditoriaLogAppService : IAuditoriaLogAppService
    {
        private readonly IAuditoriaLogsRepository _repository;

        public AuditoriaLogAppService(IAuditoriaLogsRepository repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyList<AuditoriaLogResponseDto>> LeerPorRangoFechasAsync(DateTime inicio, DateTime fin)
        {
            var logs = await _repository.ObtenerPorRangoFechasAsync(inicio, fin);

            if (!logs.Any())
            {
                throw new ExcepcionReglaNegocio("No se encontró ningun log", "LOGS_NO_ENCONTRADOS");
            }
            var listaLogs = logs.Select(log => new AuditoriaLogResponseDto
            {
                Id = log.Id,
                UsuarioId = log.UsuarioId,
                Accion = log.Accion,
                EntidadAfectada = log.EntidadAfectada,
                Fecha = log.Fecha,
                Detalles = log.Detalles
            }).ToList();

            return listaLogs;
        }

        public async Task<IReadOnlyList<AuditoriaLogResponseDto>> LeerPorUsuarioAsync(int usuarioId)
        {
            var logs = await _repository.ObtenerPorUsuarioAsync(usuarioId);

            if (!logs.Any())
            {
                throw new ExcepcionReglaNegocio("No se encontró ningun log para el usuario", "LOGS_NO_ENCONTRADOS");
            }

            return logs.Select(log => new AuditoriaLogResponseDto
            {
                Id = log.Id,
                UsuarioId = log.UsuarioId,
                Accion = log.Accion,
                EntidadAfectada = log.EntidadAfectada,
                Fecha = log.Fecha,
                Detalles = log.Detalles
            }).ToList();
        }

        public async Task<IReadOnlyList<AuditoriaLogResponseDto>> LeerTodosAsync()
        {
            var logs = await _repository.ObtenerTodosAsync();
            
            if (!logs.Any())
            {
                throw new ExcepcionReglaNegocio("No se encontró ningun log", "LOGS_NO_ENCONTRADOS");
            }

            return logs.Select(log => new AuditoriaLogResponseDto
            {
                Id = log.Id,
                UsuarioId = log.UsuarioId,
                Accion = log.Accion,
                EntidadAfectada = log.EntidadAfectada,
                Fecha = log.Fecha,
                Detalles = log.Detalles
            }).ToList();

        }
    }
}
