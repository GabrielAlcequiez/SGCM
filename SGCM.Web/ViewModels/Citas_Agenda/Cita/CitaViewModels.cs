using System.ComponentModel.DataAnnotations;

namespace SGCM.Web.ViewModels.Citas_Agenda.Cita
{
    public class CitaViewModel
    {
        public int Id { get; set; }
        public DateTime FechaHora { get; set; }
        public int Estado { get; set; }
        public string Motivo { get; set; } = string.Empty;
        public int PacienteId { get; set; }
        public int MedicoId { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

    public class CrearCitaViewModel
    {
        [Required(ErrorMessage = "El médico es requerido")]
        [Display(Name = "Médico")]
        public int MedicoId { get; set; }

        [Required(ErrorMessage = "La fecha y hora son requeridas")]
        [Display(Name = "Fecha y hora")]
        public DateTime FechaHora { get; set; }

        [Required(ErrorMessage = "El motivo es requerido")]
        [Display(Name = "Motivo")]
        public string Motivo { get; set; } = string.Empty;

        public List<MedicoDropdownViewModel> MedicosDisponibles { get; set; } = new();
    }

    public class EditarCitaViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El médico es requerido")]
        [Display(Name = "Médico")]
        public int MedicoId { get; set; }

        public int PacienteId { get; set; }

        [Required(ErrorMessage = "La fecha y hora son requeridas")]
        [Display(Name = "Fecha y hora")]
        public DateTime FechaHora { get; set; }

        [Required(ErrorMessage = "El motivo es requerido")]
        [Display(Name = "Motivo")]
        public string Motivo { get; set; } = string.Empty;

        public List<MedicoDropdownViewModel> MedicosDisponibles { get; set; } = new();
    }

    public class MedicoDropdownViewModel
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
    }

    public class PacienteDropdownViewModel
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
    }
}
