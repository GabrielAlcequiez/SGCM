using System.ComponentModel.DataAnnotations;

namespace SGCM.Web.ViewModels.Pacientes.Proveedor
{
    public class ProveedorViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Display(Name = "RNC")]
        public string RNC { get; set; } = string.Empty;

        [Display(Name = "Teléfono")]
        public string Telefono { get; set; } = string.Empty;

        [Display(Name = "Cobertura por defecto (%)")]
        public decimal CoberturaDefault { get; set; }
    }
}
