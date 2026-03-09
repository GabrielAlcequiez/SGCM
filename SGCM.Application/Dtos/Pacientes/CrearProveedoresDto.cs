namespace SGCM.Application.Dtos.Pacientes
{
    public class CrearProveedoresDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string RNC { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public decimal CoberturaDefault { get; set; }
    }
}
