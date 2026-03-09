namespace SGCM.Application.Dtos.Pacientes
{
    public class PacienteResponseDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public DateOnly FechaNacimiento { get; set; }
        public int? ProveedorId { get; set; }
        public string? NSS { get; set; }
    }
}
