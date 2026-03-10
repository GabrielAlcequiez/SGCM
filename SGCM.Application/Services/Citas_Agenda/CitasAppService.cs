using SGCM.Application.Dtos.Citas_Agenda;
using SGCM.Application.Interfaces.Citas_Agenda;
using SGCM.Application.Logger;
using SGCM.Domain.Entities.Citas_Agenda;
using SGCM.Domain.Entities.Pacientes;
using SGCM.Domain.Exceptions;
using SGCM.Domain.Repository.Citas_Agenda;
using SGCM.Domain.Services.Interfaces.ICitas;
using System.Runtime.InteropServices.Marshalling;

namespace SGCM.Application.Services.Citas_Agenda
{
    public class CitasAppService : ICitasAppService
    {
        private readonly ICitasDomainService _citasDomainService;
        private readonly ICitaRepository _citaRepository;
        private readonly IAuditoriaLogger _auditoriaLogger;
        IMedicoRepository _medicoRepository;
        IPacienteRepository _pacienteRepository;


        public CitasAppService(ICitasDomainService domainService, ICitaRepository repository, IAuditoriaLogger logger, IMedicoRepository medicoRepository, IPacienteRepository pacienteRepository)
        {
            _citasDomainService = domainService;
            _citaRepository = repository;
            _auditoriaLogger = logger;
            _medicoRepository = medicoRepository;
            _pacienteRepository = pacienteRepository;
        }

        public async Task<CitaResponseDto> CrearAsync(CrearCitaDto dto)
        {
            var medico = await _medicoRepository.ObtenerPorIdAsync(dto.MedicoId);
            if (medico is null)
                throw new ExcepcionNoEncontrado("Medico", dto.MedicoId);

            var paciente = await _pacienteRepository.ObtenerPorIdAsync(dto.PacienteId);
            if (paciente is null)
                throw new ExcepcionNoEncontrado("Paciente", dto.PacienteId);

            if (dto.FechaHora < DateTime.Now)
                throw new ExcepcionReglaNegocio("No se pueden programar citas en el pasado.", "FECHA_INVALIDAD");

            await _citasDomainService.ValidarDentroDeHorarioLaboralAsync(dto.MedicoId, dto.FechaHora);

            await _citasDomainService.ValidarHorarioDisponibleAsync(dto.MedicoId, dto.FechaHora);

            var cita = new Citas(
                dto.FechaHora,
                dto.Motivo,
                dto.PacienteId,
                dto.MedicoId
            );

            await _citaRepository.AgregarAsync(cita);

            int usuarioIdTemp = 0;
            await _auditoriaLogger.RegistrarAsync(usuarioIdTemp, "Crear", "Cita",
                $"Cita creada para el paciente {paciente.Nombre} con el Dr. {medico.Apellido} el {dto.FechaHora:dd/MM/yyyy HH:mm}");

            return new CitaResponseDto
            {
                Id = cita.Id,
                FechaHora = cita.FechaHora,
                Motivo = cita.Motivo,
                MedicoId = cita.MedicoId,
                PacienteId = cita.PacienteId, 
                Estado = cita.Estado
            };


        }
        public async Task<CitaResponseDto> ActualizarAsync(int id, CrearCitaDto dtoC)
        {
            var cita = await _citaRepository.ObtenerPorIdAsync(id);
            if (cita is null)
                throw new ExcepcionNoEncontrado("Cita", id);

            _citasDomainService.ValidarTransicionEstado(cita.Estado, cita.Estado);

            if (DateTime.Now > dtoC.FechaHora)
                throw new ExcepcionReglaNegocio("No se pueden programar citas en el pasado.", "FECHA_INVALIDA");

            if (cita.FechaHora != dtoC.FechaHora)
            {
                await _citasDomainService.ValidarDentroDeHorarioLaboralAsync(dtoC.MedicoId, dtoC.FechaHora);
                await _citasDomainService.ValidarHorarioDisponibleAsync(dtoC.MedicoId, dtoC.FechaHora);
            }

            cita.Actualizar(dtoC.FechaHora, dtoC.Motivo);
            await _citaRepository.ActualizarAsync(cita);

            int usuarioIdTemp = 0;
            await _auditoriaLogger.RegistrarAsync(usuarioIdTemp, "Actualizar", "Cita",
                $"Cita actualizada para el paciente {cita.PacienteId} con el Dr. {cita.MedicoId} el {dtoC.FechaHora:dd/MM/yyyy HH:mm}");

            return new CitaResponseDto
                {
                Id = cita.Id,
                FechaHora = cita.FechaHora,
                Motivo = cita.Motivo,
                MedicoId = cita.MedicoId,
                PacienteId = cita.PacienteId,
                Estado = cita.Estado
            };

        }
        public async Task<bool> EliminarAsync(int id)
        {
            var cita = await _citaRepository.ObtenerPorIdAsync(id);
            if (cita is null)
                throw new ExcepcionNoEncontrado("Cita", id);

            if(cita.Estado == 2)
                throw new ExcepcionReglaNegocio("No se pueden eliminar citas completadas.", "BORRADO_NO_VALIDO");

            await _citaRepository.EliminarAsync(id);

            int usuarioIdTemp = 0;
            await _auditoriaLogger.RegistrarAsync(usuarioIdTemp, "Eliminar", "Cita",
                $"Cita eliminada para el paciente {cita.PacienteId} con el Dr. {cita.MedicoId} el {cita.FechaHora}");

            return true;
        }
        public async Task<bool> CancelarCitaAsync(int id)
        {
            var cita = await _citaRepository.ObtenerPorIdAsync(id);
            if (cita is null)
                throw new ExcepcionNoEncontrado("Cita", id);

            _citasDomainService.ValidarTransicionEstado(cita.Estado, 3);

            if (cita.FechaHora <= DateTime.Now.AddHours(2))
                throw new ExcepcionReglaNegocio("No se pueden cancelar citas con menos de 2 horas de anticipación.", "CANCELACION_NO_VALIDO");

            cita.CambiarEstado(3);
            await _citaRepository.ActualizarAsync(cita);

            int usuarioIdTemp = 0;
            await _auditoriaLogger.RegistrarAsync(usuarioIdTemp, "Cancelar", "Cita",
                $"Cita cancelada para el paciente {cita.PacienteId} con el Dr. {cita.MedicoId} el {cita.FechaHora}");

            return true;
        }

        public async Task<CitaResponseDto> LeerAsync(int id)
        {
            var cita = await _citaRepository.ObtenerPorIdAsync(id);

            if (cita is null)
                throw new ExcepcionNoEncontrado("Cita", id);

            return new CitaResponseDto
            {
                Id = cita.Id,
                FechaHora = cita.FechaHora,
                Motivo = cita.Motivo,
                MedicoId = cita.MedicoId,
                PacienteId = cita.PacienteId,
                Estado = cita.Estado
            };

        }

        public async Task<IReadOnlyList<CitaResponseDto>> LeerTodosAsync()
        {
            var listaCitas = await _citaRepository.ObtenerTodosAsync();
            return listaCitas.Select(cita => new CitaResponseDto
            {
                Id = cita.Id,
                FechaHora = cita.FechaHora,
                Motivo = cita.Motivo,
                MedicoId = cita.MedicoId,
                PacienteId = cita.PacienteId,
                Estado = cita.Estado
            }).ToList();
        }

        public async Task<bool> CompletarCitaAsync(int id)
        {
            var cita = await _citaRepository.ObtenerPorIdAsync(id);

            if (cita is null)
                throw new ExcepcionNoEncontrado("Cita", id);

            _citasDomainService.ValidarTransicionEstado(cita.Estado, 2);

            if(cita.FechaHora > DateTime.Now)
                throw new ExcepcionReglaNegocio("No se pueden completar citas que aun no han sucedido.", "COMPLETADO_NO_VALIDO");

            cita.CambiarEstado(2);
            await _citaRepository.ActualizarAsync(cita);

            int usuarioIdTemp = 0;
            await _auditoriaLogger.RegistrarAsync(usuarioIdTemp, "Completar", "Cita",
                $"Cita completada para el paciente {cita.PacienteId} con el Dr. {cita.MedicoId} el {cita.FechaHora}");

            return true;
        }
    }
}
