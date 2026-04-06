using System.ComponentModel.DataAnnotations;

namespace SGCM.Web.ViewModels.Pacientes.Paciente
{
    public class PacienteViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Display(Name = "Apellido")]
        public string Apellido { get; set; } = string.Empty;

        [Display(Name = "Teléfono")]
        public string Telefono { get; set; } = string.Empty;

        [Display(Name = "Dirección")]
        public string Direccion { get; set; } = string.Empty;

        [Display(Name = "Fecha de nacimiento")]
        public DateOnly FechaNacimiento { get; set; }

        [Display(Name = "Proveedor")]
        public int? ProveedorId { get; set; }

        [Display(Name = "NSS")]
        public string? NSS { get; set; }
    }
}
