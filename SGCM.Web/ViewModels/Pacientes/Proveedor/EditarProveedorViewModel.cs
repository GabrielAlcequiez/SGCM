using System.ComponentModel.DataAnnotations;

namespace SGCM.Web.ViewModels.Pacientes.Proveedor
{
    public class EditarProveedorViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El RNC es requerido")]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "El RNC debe tener 9 dígitos")]
        [Display(Name = "RNC")]
        public string RNC { get; set; } = string.Empty;

        [Required(ErrorMessage = "El teléfono es requerido")]
        [Phone(ErrorMessage = "Ingrese un teléfono válido")]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; } = string.Empty;

        [Required(ErrorMessage = "La cobertura es requerida")]
        [Range(0, 100, ErrorMessage = "La cobertura debe estar entre 0 y 100")]
        [Display(Name = "Cobertura por defecto (%)")]
        public decimal CoberturaDefault { get; set; }
    }
}
