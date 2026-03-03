using SGCM.Domain.Base;
using SGCM.Domain.Exceptions;

namespace SGCM.Domain.Entities.Medicos
{
    public class Disponibilidad : BaseEntity, IAggregateRoot
    {
        public int DiaSemana { get; private set; } // 0=Domingo, 1=Lunes, etc.
        public TimeSpan HoraInicio { get; private set; }
        public TimeSpan HoraFin { get; private set; }
        public bool EsDiaLibre { get; private set; }
        public int MedicoId { get; private set; }

        public Disponibilidad(int diaSemana, TimeSpan horaInicio, TimeSpan horaFin, bool esDiaLibre, int medicoId)
        {
            DiaSemana = diaSemana;
            HoraInicio = horaInicio;
            HoraFin = horaFin;
            EsDiaLibre = esDiaLibre;
            MedicoId = medicoId;
            ValidarEntradaDatos();
        }

        protected override void ValidarEntradaDatos()
        {
            if (DiaSemana < 0 || DiaSemana> 6)
                throw new ExcepcionValidacion("El día de la semana debe estar entre 0 (Domingo) y 6 (Sábado).");
            if (!EsDiaLibre && HoraFin<= HoraInicio)
                throw new ExcepcionValidacion("La hora de fin debe ser mayor que la hora de inicio para días no libres.");
            if (MedicoId <= 0)
                throw new ExcepcionValidacion("Debe estar asociada a un médico válido.");
        }
    }
}
