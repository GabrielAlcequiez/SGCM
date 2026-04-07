namespace SGCM.Application.Dtos.Citas_Agenda
{
    public class FranjaHorariaDto
    {
        public string DiaSemana { get; set; } = string.Empty;
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
    }

    public class FranjasDisponiblesResponseDto
    {
        public int MedicoId { get; set; }
        public string MedicoNombre { get; set; } = string.Empty;
        public List<FranjaHorariaDto> Franjas { get; set; } = new();
    }
}
