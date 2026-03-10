namespace SGCM.Application.Dtos.Seguridad_Usuarios
{
    public class AdministradorResponseDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string? Cargo { get; set; }
    }
}
