namespace SGCM.Application.Dtos.Citas_Agenda
{
    public class CitaResponseDto
    {
        public int Id { get; set; }
        public DateTime FechaHora { get; set; }
        public int Estado { get; set; }
        public string Motivo { get; set; } = string.Empty;
        public int PacienteId { get; set; }
        public int MedicoId { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
