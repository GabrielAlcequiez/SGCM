using SGCM.Domain.Exceptions;
using SGCM.Domain.Repository.Citas_Agenda;
using SGCM.Domain.Services.Interfaces.ICitas;

namespace SGCM.Domain.Services
{
    public class CitasDomainService : ICitasDomainService
    {
        private readonly ICitaRepository _citaRepository;
        private readonly IDisponibilidadRepository _disponibilidadRepository;

        public CitasDomainService(
            ICitaRepository citaRepository,
            IDisponibilidadRepository disponibilidadRepository)
        {
            _citaRepository = citaRepository;
            _disponibilidadRepository = disponibilidadRepository;
        }

        public async Task ValidarHorarioDisponibleAsync(int medicoId, DateTime fechaHora)
        {
            var citasDelMedico = await _citaRepository.ObtenerPorMedicoAsync(medicoId);

            bool existeConflicto = await _citaRepository.ExisteCitaEnHorarioAsync(medicoId, fechaHora);

            if (existeConflicto)
            {
                throw new ExcepcionReglaNegocio(
                    $"El médico ya tiene una cita programada para el {fechaHora:dd/MM/yyyy} a las {fechaHora:HH:mm}.",
                    "CONFLICTO_HORARIO_CITA"
                );
            }
        }

        public async Task ValidarDentroDeHorarioLaboralAsync(int medicoId, DateTime fechaHora)
        {
            int diaSemana = (int)fechaHora.DayOfWeek;

            var disponibilidad = await _disponibilidadRepository.ObtenerPorMedicoYDiaAsync(medicoId, diaSemana);

            if (disponibilidad == null || disponibilidad.EsDiaLibre)
            {
                throw new ExcepcionReglaNegocio(
                    "El médico seleccionado no labora en el día especificado.",
                    "MEDICO_NO_LABORA_DIA"
                );
            }

            TimeSpan horaCita = fechaHora.TimeOfDay;
            if (horaCita < disponibilidad.HoraInicio || horaCita >= disponibilidad.HoraFin)
            {
                throw new ExcepcionReglaNegocio(
                    $"La hora solicitada ({horaCita:hh\\:mm}) está fuera del horario de trabajo del médico ({disponibilidad.HoraInicio:hh\\:mm} a {disponibilidad.HoraFin:hh\\:mm}).",
                    "FUERA_HORARIO_LABORAL"
                );
            }
        }

        public void ValidarTransicionEstado(int estadoActual, int nuevoEstado)
        {
            if (estadoActual == 2)
            {
                throw new ExcepcionReglaNegocio(
                    "No se puede modificar ni cancelar una cita que ya ha sido completada.",
                    "CITA_COMPLETADA_INMUTABLE"
                );
            }

            if (estadoActual == 3)
            {
                throw new ExcepcionReglaNegocio(
                    "No se puede reabrir ni completar una cita que ya fue cancelada. Debe agendar una nueva.",
                    "CITA_CANCELADA_INMUTABLE"
                );
            }
        }
    }
}
