namespace SGCM.Application.Dtos.Seguridad_Usuarios
{
    public class AuditoriaLogResponseDto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string Accion { get; set; } = string.Empty;
        public string EntidadAfectada { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public string Detalles { get; set; } = string.Empty;
    }
}
