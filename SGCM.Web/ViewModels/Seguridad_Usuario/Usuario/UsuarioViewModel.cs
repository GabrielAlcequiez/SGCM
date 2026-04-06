using System.ComponentModel.DataAnnotations;

namespace SGCM.Web.ViewModels.Seguridad_Usuario.Usuario
{
    public class UsuarioViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El correo electrónico es requerido")]
        [EmailAddress(ErrorMessage = "Ingrese un correo electrónico válido")]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Rol")]
        public string Rol { get; set; } = string.Empty;

        [Display(Name = "Fecha de registro")]
        public DateTime FechaRegistro { get; set; }
    }
}
