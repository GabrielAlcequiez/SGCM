namespace SGCM.Application.Dtos.Citas_Agenda
{
    public class DisponibilidadResponseDto
    {
        public int Id { get; set; }
        public int MedicoId { get; set; }
        public int DiaSemana { get; set; } // 0=Domingo, 1=Lunes, etc.
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public bool EsDiaLibre { get; set; }
    }
}
