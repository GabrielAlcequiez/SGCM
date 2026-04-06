using System.ComponentModel.DataAnnotations;

namespace SGCM.Web.ViewModels.Citas_Agenda.Disponibilidad
{
    public class DisponibilidadViewModel
    {
        public int Id { get; set; }
        public int MedicoId { get; set; }
        public int DiaSemana { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public bool EsDiaLibre { get; set; }
    }

    public class CrearDisponibilidadViewModel
    {
        [Required(ErrorMessage = "El médico es requerido")]
        [Display(Name = "Médico")]
        public int MedicoId { get; set; }

        [Required(ErrorMessage = "El día de la semana es requerido")]
        [Display(Name = "Día de la semana")]
        public int DiaSemana { get; set; }

        [Required(ErrorMessage = "La hora de inicio es requerida")]
        [Display(Name = "Hora de inicio")]
        public TimeSpan HoraInicio { get; set; }

        [Required(ErrorMessage = "La hora de fin es requerida")]
        [Display(Name = "Hora de fin")]
        public TimeSpan HoraFin { get; set; }

        [Display(Name = "Es día libre")]
        public bool EsDiaLibre { get; set; }

        public List<MedicoDropdownViewModel> MedicosDisponibles { get; set; } = new();
        public List<DiaSemanaOption> DiasSemana { get; set; } = new();
    }

    public class EditarDisponibilidadViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El médico es requerido")]
        [Display(Name = "Médico")]
        public int MedicoId { get; set; }

        [Required(ErrorMessage = "El día de la semana es requerido")]
        [Display(Name = "Día de la semana")]
        public int DiaSemana { get; set; }

        [Required(ErrorMessage = "La hora de inicio es requerida")]
        [Display(Name = "Hora de inicio")]
        public TimeSpan HoraInicio { get; set; }

        [Required(ErrorMessage = "La hora de fin es requerida")]
        [Display(Name = "Hora de fin")]
        public TimeSpan HoraFin { get; set; }

        [Display(Name = "Es día libre")]
        public bool EsDiaLibre { get; set; }

        public List<MedicoDropdownViewModel> MedicosDisponibles { get; set; } = new();
        public List<DiaSemanaOption> DiasSemana { get; set; } = new();
    }

    public class MedicoDropdownViewModel
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
    }

    public class DiaSemanaOption
    {
        public int Valor { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }
}
