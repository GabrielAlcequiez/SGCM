namespace SGCM.Application.Dtos.Seguridad_Usuarios
{
    public class CrearUsuarioDto
    {
        public string email { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
    }
}
