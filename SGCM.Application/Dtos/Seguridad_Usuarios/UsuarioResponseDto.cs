namespace SGCM.Application.Dtos.Seguridad_Usuarios
{
    public class UsuarioResponseDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; }
    }
}
