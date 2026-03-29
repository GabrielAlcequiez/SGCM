using SGCM.Application.Dtos.Citas_Agenda;
using SGCM.Application.Interfaces.Citas_Agenda;
using SGCM.Application.Logger;
using SGCM.Domain.Entities.Medicos;
using SGCM.Domain.Exceptions;
using SGCM.Domain.Repository;
using SGCM.Domain.Repository.Citas_Agenda;
using SGCM.Domain.Services.Interfaces.ICitas;
using SGCM.Infraestructure.Services;

namespace SGCM.Application.Services.Citas_Agenda
{
    public class MedicoAppService : IMedicoAppService
    {
        private readonly IMedicoRepository _repository;
        private readonly IMedicoDomainService _domainService;
        private readonly IAuditoriaLogger _auditoriaLogger;
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;

        public MedicoAppService(IAuditoriaLogger auditoriaLogger, IMedicoRepository repository, IMedicoDomainService domainService, ITokenService tokenService, IUnitOfWork unitOfWork)
        {
            _auditoriaLogger = auditoriaLogger;
            _repository = repository;
            _domainService = domainService;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
        }

        public async Task<MedicoResponseDto> CrearAsync(CrearMedicoDto dto)
        {
            var usuarioIdActual = _tokenService.ObtenerUsuarioIdActual();
            await _domainService.UsuarioYaTienePerfilMedicoAsync(usuarioIdActual);
            await _domainService.EsExequaturUnicoAsync(dto.Exequatur);

            var medico = new Medico
                (dto.Nombre, 
                dto.Apellido,
                dto.Exequatur, 
                dto.Telefono,
                dto.EspecialidadId,
                usuarioIdActual);

            await _repository.AgregarAsync(medico);

            await _unitOfWork.CommitAsync(
                postCommitAction: async () => await _auditoriaLogger.RegistrarAsync(usuarioIdActual, "Crear", "Medico", $"Se creó un nuevo médico con ID: {medico.Id}")
            );

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

            await _unitOfWork.CommitAsync(
                postCommitAction: async () =>
                {
                    var usuarioIdActual = _tokenService.ObtenerUsuarioIdActual();
                    await _auditoriaLogger.RegistrarAsync(usuarioIdActual, "Actualizar", "Medico", $"Se actualizó el médico con ID: {medico.Id}");
                }
            );

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

            medico.Eliminar();
            await _repository.ActualizarAsync(medico);

            await _unitOfWork.CommitAsync(
                postCommitAction: async () =>
                {
                    var usuarioIdActual = _tokenService.ObtenerUsuarioIdActual();
                    await _auditoriaLogger.RegistrarAsync(usuarioIdActual, "Eliminar", "Medico", $"Se eliminó el médico con ID: {id}");
                }
            );

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
                EspecialidadId = medico.EspecialidadId,
                UsuarioId = medico.UsuarioId
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
                EspecialidadId = m.EspecialidadId,
                UsuarioId = m.UsuarioId
            }).ToList();
        }
    }
}
