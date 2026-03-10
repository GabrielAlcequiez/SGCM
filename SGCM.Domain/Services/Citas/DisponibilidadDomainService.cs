using SGCM.Domain.Exceptions;
using SGCM.Domain.Repository.Citas_Agenda;
using SGCM.Domain.Services.Interfaces.ICitas;

namespace SGCM.Domain.Services
{
    public class DisponibilidadDomainService : IDisponibilidadDomainService
    {
        private readonly IDisponibilidadRepository _disponibilidadRepository;
        private readonly IMedicoRepository _medicoRepository;

        public DisponibilidadDomainService(
            IDisponibilidadRepository disponibilidadRepository,
            IMedicoRepository medicoRepository)
        {
            _disponibilidadRepository = disponibilidadRepository;
            _medicoRepository = medicoRepository;
        }

        public async Task ValidarDisponibilidadUnicaPorDiaAsync(int medicoId, int diaSemana)
        {
            var medico = await _medicoRepository.ObtenerPorIdAsync(medicoId);
            if (medico == null)
            {
                throw new ExcepcionNoEncontrado("Médico", medicoId);
            }

            var disponibilidadExistente = await _disponibilidadRepository.ObtenerPorMedicoYDiaAsync(medicoId, diaSemana);

            if (disponibilidadExistente != null)
            {
                string nombreDia = ObtenerNombreDia(diaSemana);

                throw new ExcepcionReglaNegocio(
                    $"El médico ya tiene un horario configurado para el día {nombreDia}. Si desea cambiarlo, debe actualizar el registro existente.",
                    "DISPONIBILIDAD_DIA_DUPLICADA"
                );
            }
        }

        // Para mejor legibilidad en los mensajes de error, convertimos el número del día de la semana a su nombre correspondiente
        public string ObtenerNombreDia(int diaSemana)
        {
            return diaSemana switch
            {
                0 => "Domingo",
                1 => "Lunes",
                2 => "Martes",
                3 => "Miércoles",
                4 => "Jueves",
                5 => "Viernes",
                6 => "Sábado"
            };
        }

        public void ValidarConsistenciaHorario(TimeSpan inicio, TimeSpan fin, bool esDiaLibre)
        {
            if (esDiaLibre)
                return;

            if (!esDiaLibre && fin <= inicio)
            {
                throw new ExcepcionValidacion("La hora de fin debe ser mayor que la hora de inicio para días no libres.");
            }
        }
    }
}
