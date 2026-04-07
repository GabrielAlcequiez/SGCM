using System.ComponentModel.DataAnnotations;

namespace SGCM.Web.ViewModels.Citas_Agenda.Medico
{
    public class MedicoViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Exequatur { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public int EspecialidadId { get; set; }
        public int UsuarioId { get; set; }
    }

    public class CrearMedicoViewModel
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es requerido")]
        [Display(Name = "Apellido")]
        public string Apellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "La especialidad es requerida")]
        [Display(Name = "Especialidad")]
        public int EspecialidadId { get; set; }

        [Required(ErrorMessage = "El exequatur es requerido")]
        [Display(Name = "Exequatur")]
        public string Exequatur { get; set; } = string.Empty;

        [Required(ErrorMessage = "El teléfono es requerido")]
        [Phone(ErrorMessage = "Ingrese un teléfono válido")]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; } = string.Empty;

        public List<EspecialidadDropdownViewModel> EspecialidadesDisponibles { get; set; } = new();
    }

    public class EditarMedicoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es requerido")]
        [Display(Name = "Apellido")]
        public string Apellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "La especialidad es requerida")]
        [Display(Name = "Especialidad")]
        public int EspecialidadId { get; set; }

        [Required(ErrorMessage = "El exequatur es requerido")]
        [Display(Name = "Exequatur")]
        public string Exequatur { get; set; } = string.Empty;

        [Required(ErrorMessage = "El teléfono es requerido")]
        [Phone(ErrorMessage = "Ingrese un teléfono válido")]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; } = string.Empty;

        public List<EspecialidadDropdownViewModel> EspecialidadesDisponibles { get; set; } = new();
    }

    public class EspecialidadDropdownViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }

    public class CompletarPerfilMedicoViewModel
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

        [Display(Name = "Especialidad")]
        public int? EspecialidadId { get; set; }

        [Required(ErrorMessage = "El exequatur es requerido")]
        [Display(Name = "Exequatur")]
        public string Exequatur { get; set; } = string.Empty;

        public List<EspecialidadDropdownViewModel> EspecialidadesDisponibles { get; set; } = new();
    }
}
