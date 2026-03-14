using SGCM.Application.Dtos.Citas_Agenda;
using SGCM.Application.Interfaces;
using SGCM.Application.Interfaces.Citas_Agenda;
using SGCM.Application.Logger;
using SGCM.Domain.Entities.Medicos;
using SGCM.Domain.Exceptions;
using SGCM.Domain.Repository.Citas_Agenda;
using SGCM.Domain.Services.Interfaces.ICitas;

namespace SGCM.Application.Services.Citas_Agenda
{
    public class DisponibilidadAppService : IDisponibilidadAppService
    {
        private readonly IDisponibilidadRepository _repository;
        private readonly IDisponibilidadDomainService _domainService;
        private readonly IAuditoriaLogger _auditoriaLogger;
        private readonly ITokenService _tokenService;
        public DisponibilidadAppService(IDisponibilidadRepository repository, IDisponibilidadDomainService domainService, IAuditoriaLogger auditoriaLogger, ITokenService tokenService)
        {
            _repository = repository;
            _domainService = domainService;
            _auditoriaLogger = auditoriaLogger;
            _tokenService = tokenService;
        }

        public async Task<DisponibilidadResponseDto> CrearAsync(CrearDisponibilidadDto dto)
        {
            await _domainService.ValidarDisponibilidadUnicaPorDiaAsync(dto.IdMedico, dto.DiaSemana);
            _domainService.ValidarConsistenciaHorario(dto.HoraInicio, dto.HoraFin, dto.EsDiaLibre);

            var dispo = new Disponibilidad
            (
                dto.DiaSemana,
                dto.HoraInicio,
                dto.HoraFin,
                dto.EsDiaLibre,
                dto.IdMedico
            );

            await _repository.AgregarAsync(dispo);
            var usuarioIdActual = _tokenService.ObtenerUsuarioIdActual();
            string dia = _domainService.ObtenerNombreDia(dto.DiaSemana);
            await _auditoriaLogger.RegistrarAsync(usuarioIdActual, "Crear", "Disponibilidad", $"Agenda de Disponibilidad creada para dia {dia}.");

            return new DisponibilidadResponseDto
            {
                Id = dispo.Id,
                MedicoId = dispo.MedicoId,
                DiaSemana = dispo.DiaSemana,
                HoraInicio = dispo.HoraInicio,
                HoraFin = dispo.HoraFin,
                EsDiaLibre = dispo.EsDiaLibre
            };


        }

        public async Task<DisponibilidadResponseDto> ActualizarAsync(int id, CrearDisponibilidadDto dtoC)
        {
            var dispo = await _repository.ObtenerPorIdAsync(id);
            if (dispo is null)
                throw new ExcepcionNoEncontrado("Disponibilidad", id);

            if (dispo.DiaSemana != dtoC.DiaSemana)
            {
                await _domainService.ValidarDisponibilidadUnicaPorDiaAsync(dtoC.IdMedico, dtoC.DiaSemana);
            }

            _domainService.ValidarConsistenciaHorario(dtoC.HoraInicio, dtoC.HoraFin, dtoC.EsDiaLibre);

            dispo.Actualizar(dtoC.DiaSemana, dtoC.HoraInicio, dtoC.HoraFin, dtoC.EsDiaLibre);
            await _repository.ActualizarAsync(dispo);

            var usuarioIdActual = _tokenService.ObtenerUsuarioIdActual();
            string dia = _domainService.ObtenerNombreDia(dtoC.DiaSemana);
            await _auditoriaLogger.RegistrarAsync(usuarioIdActual, "Actualizar", "Disponibilidad", $"Disponibilidad ID: {id} actualizada para el médico ID: {dispo.MedicoId} el día {dia}.");

            return new DisponibilidadResponseDto
            {
                Id = dispo.Id,
                MedicoId = dispo.MedicoId,
                DiaSemana = dispo.DiaSemana,
                HoraInicio = dispo.HoraInicio,
                HoraFin = dispo.HoraFin,
                EsDiaLibre = dispo.EsDiaLibre
            };
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var dispo = await _repository.ObtenerPorIdAsync(id);
            if (dispo is null)
                throw new ExcepcionNoEncontrado("Disponibilidad", id);

            await _domainService.PuedeEliminarCitaAsync(dispo.Id);

            await _repository.EliminarAsync(id);

            var usuarioIdActual = _tokenService.ObtenerUsuarioIdActual();
            string dia = _domainService.ObtenerNombreDia(dispo.DiaSemana);
            await _auditoriaLogger.RegistrarAsync(usuarioIdActual, "Eliminar", "Disponibilidad", $"Disponibilidad ID: {id} eliminada para el médico ID: {dispo.MedicoId} el día {dia}.");
            return true;
        }

        public async Task<DisponibilidadResponseDto> LeerAsync(int id)
        {
            var dispo = await _repository.ObtenerPorIdAsync(id);
            if (dispo is null)
                throw new ExcepcionNoEncontrado("Disponibilidad", id);

            return new DisponibilidadResponseDto
            {
                Id = dispo.Id,
                MedicoId = dispo.MedicoId,
                DiaSemana = dispo.DiaSemana,
                HoraInicio = dispo.HoraInicio,
                HoraFin = dispo.HoraFin,
                EsDiaLibre = dispo.EsDiaLibre
            };
        }

        public async Task<IReadOnlyList<DisponibilidadResponseDto>> LeerTodosAsync()
        {
            var listaDisponibilidad = await _repository.ObtenerTodosAsync();
            return listaDisponibilidad.Select(dispo => new DisponibilidadResponseDto
            {
                Id = dispo.Id,
                MedicoId = dispo.MedicoId,
                DiaSemana = dispo.DiaSemana,
                HoraInicio = dispo.HoraInicio,
                HoraFin = dispo.HoraFin,
                EsDiaLibre = dispo.EsDiaLibre
            }).ToList();
        }
    }
}
