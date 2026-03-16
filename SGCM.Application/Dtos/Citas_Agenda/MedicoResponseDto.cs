namespace SGCM.Application.Dtos.Citas_Agenda
{
    public class MedicoResponseDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Exequatur { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public int EspecialidadId { get; set; }

        public int UsuarioId { get; set; }
    }
}
