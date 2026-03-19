using SGCM.Domain.Base;
using SGCM.Domain.Exceptions;
using SGCM.Domain.Validaciones;

namespace SGCM.Domain.Entities.Citas_Agenda
{
    public class Citas : DeletableEntity
    {
        public DateTime FechaHora { get; private set; }
        public int Estado { get; private set; }
        public string Motivo { get; private set; }
        public int PacienteId { get; private set; }
        public int MedicoId { get; private set; }
        public DateTime FechaCreacion { get; private set; }

        protected Citas() { }

        public Citas(DateTime fechaHora, string motivo, int pacienteId, int medicoId)
        {
            
            FechaHora = fechaHora;
            Estado = 1;
            Motivo = motivo;
            PacienteId = pacienteId;
            MedicoId = medicoId;
            FechaCreacion = DateTime.Now;
            ValidarEntradaDatos();
        }

        protected override void ValidarEntradaDatos()
        {
            ValidacionBase<Citas>.IdValido(PacienteId, "PacienteId");
            ValidacionBase<Citas>.IdValido(MedicoId, "MedicoId");

            ValidacionBase<Citas>.FechaHoraNoPasada(FechaHora, "FechaHora");

            ValidacionBase<Citas>.Requerido(Motivo, "Motivo");
            ValidacionBase<Citas>.Longitud(Motivo, 200, "Motivo");
        }

        public void Actualizar(DateTime fechaHora, string motivo)
        {
            FechaHora = fechaHora;
            Motivo = motivo;
            ValidarEntradaDatos();
        }

        public void CambiarEstado(int nuevoEstado)
        {
            Estado = nuevoEstado;
        }

    }
}
