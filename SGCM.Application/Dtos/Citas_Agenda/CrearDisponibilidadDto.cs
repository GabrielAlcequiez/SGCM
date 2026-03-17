namespace SGCM.Application.Dtos.Citas_Agenda
{
    public class CrearDisponibilidadDto
    {
        public int DiaSemana { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public bool EsDiaLibre { get; set; }
    }
}
