using SGCM.Application.Dtos.Citas_Agenda;
using SGCM.Application.Interfaces;
using SGCM.Application.Logger;
using SGCM.Domain.Entities.Medicos;
using SGCM.Domain.Exceptions;
using SGCM.Domain.Repository.Citas_Agenda;
using SGCM.Domain.Services.Interfaces.ICitas;

namespace SGCM.Application.Services
{
    public class MedicoAppService : IMedicoAppService
    {
        private readonly IMedicoRepository _repository;
        private readonly IMedicoDomainService _domainService;
        private readonly IAuditoriaLogger _auditoriaLogger;

        public MedicoAppService(IAuditoriaLogger auditoriaLogger, IMedicoRepository repository, IMedicoDomainService domainService)
        {
            _auditoriaLogger = auditoriaLogger;
            _repository = repository;
            _domainService = domainService;
        }

        public async Task<MedicoResponseDto> CrearAsync(CrearMedicoDto dto)
        {
            await _domainService.UsuarioYaTienePerfilMedicoAsync(dto.UsuarioId);
            await _domainService.EsExequaturUnicoAsync(dto.Exequatur);

            var medico = new Medico
                (dto.Nombre, 
                dto.Apellido,
                dto.Exequatur, 
                dto.Telefono,
                dto.EspecialidadId,
                dto.UsuarioId);

            await _repository.AgregarAsync(medico);
            int usuarioIdTemp = 0;
            await _auditoriaLogger.RegistrarAsync(usuarioIdTemp, "Crear", "Medico", $"Se creó un nuevo médico con ID: {medico.Id}" );

            return new MedicoResponseDto
            {
                Id = medico.Id,
                Nombre = medico.Nombre,
                Apellido = medico.Apellido,
                Exequatur = medico.Exequatur,
                Telefono = medico.Telefono,
                EspecialidadId = medico.EspecialidadId
            };
        }

        public async Task<MedicoResponseDto> ActualizarAsync(int id, CrearMedicoDto dtoC)
        {
            var medico = await _repository.ObtenerPorIdAsync(id);
            if (medico is null)
            {
                throw new ExcepcionNoEncontrado("Médico", id);
            }

            if (medico.Exequatur != dtoC.Exequatur)
            {
                await _domainService.EsExequaturUnicoAsync(dtoC.Exequatur);
            }

            medico.Actualizar
                (dtoC.Nombre, 
                dtoC.Apellido, 
                dtoC.Exequatur, 
                dtoC.Telefono, 
                dtoC.EspecialidadId);

            await _repository.ActualizarAsync(medico);
            int usuarioIdTemp = 0;
            await _auditoriaLogger.RegistrarAsync(usuarioIdTemp, "Actualizar", "Medico", $"Se actualizó el médico con ID: {medico.Id}" );

            return new MedicoResponseDto
            {
                Id = medico.Id,
                Nombre = medico.Nombre,
                Apellido = medico.Apellido,
                Exequatur = medico.Exequatur,
                Telefono = medico.Telefono,
                EspecialidadId = medico.EspecialidadId
            };
        }


        public async Task<bool> EliminarAsync(int id)
        {
            var medico = await _repository.ObtenerPorIdAsync(id);
            
            if (medico is null)
            {
                throw new ExcepcionNoEncontrado("Médico", id);
            }
            await _domainService.PuedeEliminarMedicoAsync(id);

            await _repository.EliminarAsync(id);
            int usuarioIdTemp = 0;
            await _auditoriaLogger.RegistrarAsync(usuarioIdTemp, "Eliminar", "Medico", $"Se eliminó el médico con ID: {id}" );

            return true;
        }

        public async Task<MedicoResponseDto> LeerAsync(int id)
        {
            var medico = await _repository.ObtenerPorIdAsync(id);
            if (medico is null)
            {
                throw new ExcepcionNoEncontrado("Médico", id);
            }

            return new MedicoResponseDto
            {
                Id = medico.Id,
                Nombre = medico.Nombre,
                Apellido = medico.Apellido,
                Exequatur = medico.Exequatur,
                Telefono = medico.Telefono,
                EspecialidadId = medico.EspecialidadId
            };
        }

        public async Task<IReadOnlyList<MedicoResponseDto>> LeerTodosAsync()
        {
            var listaMedicos = await _repository.ObtenerTodosAsync();

            return listaMedicos.Select(m => new MedicoResponseDto
            {
                Id = m.Id,
                Nombre = m.Nombre,
                Apellido = m.Apellido,
                Exequatur = m.Exequatur,
                Telefono = m.Telefono,
                EspecialidadId = m.EspecialidadId
            }).ToList();
        }
    }
}
