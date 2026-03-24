using SGCM.Application.Dtos.Citas_Agenda;
using SGCM.Application.Interfaces;
using SGCM.Application.Interfaces.Citas_Agenda;
using SGCM.Application.Logger;
using SGCM.Domain.Entities.Citas_Agenda;
using SGCM.Domain.Entities.Pacientes;
using SGCM.Domain.Exceptions;
using SGCM.Domain.Repository;
using SGCM.Domain.Repository.Citas_Agenda;
using SGCM.Domain.Services.Interfaces.ICitas;

namespace SGCM.Application.Services.Citas_Agenda
{
    public class CitasAppService : ICitasAppService
    {
        private readonly ICitasDomainService _citasDomainService;
        private readonly ICitaRepository _citaRepository;
        private readonly IAuditoriaLogger _auditoriaLogger;
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificacionService _notificacionService;
        IMedicoRepository _medicoRepository;
        IPacienteRepository _pacienteRepository;

        public CitasAppService(ICitasDomainService domainService, ICitaRepository repository, IAuditoriaLogger logger, IMedicoRepository medicoRepository, IPacienteRepository pacienteRepository, ITokenService tokenService, IUnitOfWork unitOfWork, INotificacionService notificacionService)
        {
            _citasDomainService = domainService;
            _citaRepository = repository;
            _auditoriaLogger = logger;
            _medicoRepository = medicoRepository;
            _pacienteRepository = pacienteRepository;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
            _notificacionService = notificacionService;
        }

        public async Task<CitaResponseDto> CrearAsync(CrearCitaDto dto)
        {
            var usuarioIdActual = _tokenService.ObtenerUsuarioIdActual();

            var medico = await _medicoRepository.ObtenerPorIdAsync(dto.MedicoId);
            if (medico is null)
                throw new ExcepcionNoEncontrado("Medico", dto.MedicoId);

            var paciente = await _pacienteRepository.ObtenerPorUsuarioIdAsync(usuarioIdActual);
            if (paciente is null)
                throw new ExcepcionNoEncontrado("Paciente", usuarioIdActual);

            if (dto.FechaHora < DateTime.Now)
                throw new ExcepcionReglaNegocio("No se pueden programar citas en el pasado.", "FECHA_INVALIDAD");

            await _citasDomainService.ValidarDentroDeHorarioLaboralAsync(dto.MedicoId, dto.FechaHora);

            await _citasDomainService.ValidarHorarioDisponibleAsync(dto.MedicoId, dto.FechaHora);

            var cita = new Citas(
                dto.FechaHora,
                dto.Motivo,
                paciente.Id,
                dto.MedicoId
            );

            await _citaRepository.AgregarAsync(cita);

            await _unitOfWork.CommitAsync(
                postCommitAction: async () =>
                {
                    await _auditoriaLogger.RegistrarAsync(usuarioIdActual, "Crear", "Cita",
                        $"Cita creada para el paciente {paciente.Nombre} con el Dr. {medico.Apellido} el {dto.FechaHora:dd/MM/yyyy HH:mm}");

                    await _notificacionService.EnviarAsync(
                        new[] { paciente.UsuarioId },
                        "CitaAgendada",
                        $"Su cita ha sido programada para el {cita.FechaHora:dd/MM/yyyy HH:mm} con el/la Dr/Dra. {medico.Nombre} {medico.Apellido}."
                    );
                }
            );

            return new CitaResponseDto
            {
                Id = cita.Id,
                FechaHora = cita.FechaHora,
                Motivo = cita.Motivo,
                MedicoId = cita.MedicoId,
                PacienteId = cita.PacienteId, 
                Estado = cita.Estado,
                FechaCreacion = cita.FechaCreacion
            };
        }

        public async Task<CitaResponseDto> ActualizarAsync(int id, CrearCitaDto dtoC)
        {
            var cita = await _citaRepository.ObtenerPorIdAsync(id);
            if (cita is null)
                throw new ExcepcionNoEncontrado("Cita", id);

            _citasDomainService.ValidarTransicionEstado(cita.Estado);

            if (DateTime.Now > dtoC.FechaHora)
                throw new ExcepcionReglaNegocio("No se pueden programar citas en el pasado.", "FECHA_INVALIDA");

            if (cita.FechaHora != dtoC.FechaHora)
            {
                await _citasDomainService.ValidarDentroDeHorarioLaboralAsync(dtoC.MedicoId, dtoC.FechaHora);
                await _citasDomainService.ValidarHorarioDisponibleAsync(dtoC.MedicoId, dtoC.FechaHora);
            }

            cita.Actualizar(dtoC.FechaHora, dtoC.Motivo);
            await _citaRepository.ActualizarAsync(cita);

            await _unitOfWork.CommitAsync(
                postCommitAction: async () =>
                {
                    var usuarioIdActual = _tokenService.ObtenerUsuarioIdActual();
                    await _auditoriaLogger.RegistrarAsync(usuarioIdActual, "Actualizar", "Cita",
                        $"Cita actualizada para el paciente {cita.PacienteId} con el Dr. {cita.MedicoId} el {dtoC.FechaHora:dd/MM/yyyy HH:mm}");

                    var pacienteActualizado = await _pacienteRepository.ObtenerPorIdAsync(cita.PacienteId);
                    var medicoActualizado = await _medicoRepository.ObtenerPorIdAsync(cita.MedicoId);
                    if (pacienteActualizado is not null && medicoActualizado is not null)
                    {
                        await _notificacionService.EnviarAsync(
                            new[] { pacienteActualizado.UsuarioId, medicoActualizado.UsuarioId },
                            "CitaModificada",
                            $"Su cita del {cita.FechaHora:dd/MM/yyyy HH:mm} ha sido modificada."
                        );
                    }
                }
            );

            return new CitaResponseDto
                {
                Id = cita.Id,
                FechaHora = cita.FechaHora,
                Motivo = cita.Motivo,
                MedicoId = cita.MedicoId,
                PacienteId = cita.PacienteId,
                Estado = cita.Estado,
                FechaCreacion = cita.FechaCreacion
            };

        }

        public async Task<bool> EliminarAsync(int id)
        {
            var cita = await _citaRepository.ObtenerPorIdAsync(id);
            if (cita is null)
                throw new ExcepcionNoEncontrado("Cita", id);

            if(cita.Estado == 2)
                throw new ExcepcionReglaNegocio("No se pueden eliminar citas completadas.", "BORRADO_NO_VALIDO");

            cita.Eliminar();
            await _citaRepository.ActualizarAsync(cita);

            await _unitOfWork.CommitAsync(
                postCommitAction: async () =>
                {
                    var usuarioIdActual = _tokenService.ObtenerUsuarioIdActual();
                    await _auditoriaLogger.RegistrarAsync(usuarioIdActual, "Eliminar", "Cita",
                        $"Cita eliminada para el paciente {cita.PacienteId} con el Dr. {cita.MedicoId} el {cita.FechaHora}");
                }
            );

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

            await _unitOfWork.CommitAsync(
                postCommitAction: async () =>
                {
                    var usuarioIdActual = _tokenService.ObtenerUsuarioIdActual();
                    await _auditoriaLogger.RegistrarAsync(usuarioIdActual, "Cancelar", "Cita",
                        $"Cita cancelada para el paciente {cita.PacienteId} con el Dr. {cita.MedicoId} el {cita.FechaHora}");

                    var pacienteCancelado = await _pacienteRepository.ObtenerPorIdAsync(cita.PacienteId);
                    var medicoCancelado = await _medicoRepository.ObtenerPorIdAsync(cita.MedicoId);
                    if (pacienteCancelado is not null && medicoCancelado is not null)
                    {
                        await _notificacionService.EnviarAsync(
                            new[] { pacienteCancelado.UsuarioId, medicoCancelado.UsuarioId },
                            "CitaCancelada",
                            $"La cita del {cita.FechaHora:dd/MM/yyyy HH:mm} ha sido cancelada."
                        );
                    }
                }
            );

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
                Estado = cita.Estado,
                FechaCreacion = cita.FechaCreacion
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
                Estado = cita.Estado,
                FechaCreacion = cita.FechaCreacion
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

            await _unitOfWork.CommitAsync(
                postCommitAction: async () =>
                {
                    var usuarioIdActual = _tokenService.ObtenerUsuarioIdActual();
                    await _auditoriaLogger.RegistrarAsync(usuarioIdActual, "Completar", "Cita",
                        $"Cita completada para el paciente {cita.PacienteId} con el Dr. {cita.MedicoId} el {cita.FechaHora}");

                    var pacienteCompletado = await _pacienteRepository.ObtenerPorIdAsync(cita.PacienteId);
                    if (pacienteCompletado is not null)
                    {
                        await _notificacionService.EnviarAsync(
                            new[] { pacienteCompletado.UsuarioId },
                            "CitaCompletada",
                            $"Su cita del {cita.FechaHora:dd/MM/yyyy HH:mm} ha sido completada."
                        );
                    }
                }
            );

            return true;
        }
    }
}
