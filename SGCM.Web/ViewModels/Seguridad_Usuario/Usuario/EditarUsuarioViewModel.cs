using System.ComponentModel.DataAnnotations;

namespace SGCM.Web.ViewModels.Seguridad_Usuario.Usuario
{
    public class EditarUsuarioViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El correo electrónico es requerido")]
        [EmailAddress(ErrorMessage = "Ingrese un correo electrónico válido")]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "El rol es requerido")]
        [Display(Name = "Rol")]
        public string Rol { get; set; } = string.Empty;

        public List<string> RolesDisponibles { get; set; } = new() { "Paciente", "Medico", "Administrador" };
    }
}
