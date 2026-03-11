namespace SGCM.Application.Dtos.Login
{
    public class LoginResponseDto
    {
        public int UsuarioId { get; set; }
        public string Email{ get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
