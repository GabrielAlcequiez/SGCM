using SGCM.Domain.Base;
using SGCM.Domain.Exceptions;
using System.Runtime.InteropServices.Marshalling;

namespace SGCM.Domain.Entities.Citas_Agenda
{
    public class Citas : BaseEntity
    {
        public DateTime FechaHora { get; private set; }
        public int Estado { get; private set; }
        public string Motivo { get; private set; }
        public int PacienteId { get; private set; }
        public int MedicoId { get; private set; }
        public DateTime FechaCreacion { get; private set; }

        public Citas(DateTime fechaHora, int estado, string motivo, int pacienteId, int medicoId)
        {
            
            FechaHora = fechaHora;
            Estado = estado;
            Motivo = motivo;
            PacienteId = pacienteId;
            MedicoId = medicoId;
            FechaCreacion = DateTime.Now;
            ValidarEntradaDatos();
        }

        protected override void ValidarEntradaDatos()
        {
            if (PacienteId <= 0 || MedicoId <= 0)
                throw new ExcepcionValidacion("Los IDs de paciente y médico deben ser válidos.");

            if (FechaHora < DateTime.Now)
                throw new ExcepcionValidacion("La fecha de la cita no puede ser anterior a la fecha actual.");

            if (string.IsNullOrEmpty(Motivo))
                throw new ExcepcionValidacion("El motivo de la cita no puede estar vacío.");
            if (Motivo.Length > 200)
                throw new ExcepcionValidacion("El motivo no puede exceder los 200 caracteres.");
        }
    }
}
