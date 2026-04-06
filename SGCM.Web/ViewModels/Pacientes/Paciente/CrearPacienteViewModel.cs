using System.ComponentModel.DataAnnotations;

namespace SGCM.Web.ViewModels.Pacientes.Paciente
{
    public class CrearPacienteViewModel
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es requerido")]
        [Display(Name = "Apellido")]
        public string Apellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "El teléfono es requerido")]
        [Phone(ErrorMessage = "Ingrese un teléfono válido")]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; } = string.Empty;

        [Required(ErrorMessage = "La dirección es requerida")]
        [Display(Name = "Dirección")]
        public string Direccion { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de nacimiento es requerida")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de nacimiento")]
        public DateOnly FechaNacimiento { get; set; }

        [Display(Name = "Proveedor/ARS")]
        public int? ProveedorId { get; set; }

        [Display(Name = "NSS")]
        public string? NSS { get; set; }

        public List<ProveedorDropdownViewModel> ProveedoresDisponibles { get; set; } = new();
    }

    public class ProveedorDropdownViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }
}
