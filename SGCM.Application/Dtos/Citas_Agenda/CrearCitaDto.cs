namespace SGCM.Application.Dtos.Citas_Agenda
{
    public class CrearCitaDto
    {
        public int MedicoId { get; set; }
        public DateTime FechaHora { get; set; }
        public string Motivo { get; set; } = string.Empty;

    }
}
