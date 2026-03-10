namespace SGCM.Application.Dtos.Citas_Agenda
{
    public class CrearMedicoDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public int EspecialidadId { get; set; } 
        public string Exequatur { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public int UsuarioId { get; set; }
    }
}
