using System.ComponentModel.DataAnnotations;

namespace SGCM.Web.ViewModels.Seguridad_Usuario.Auditoria
{
    public class AuditoriaViewModel
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }

        [Display(Name = "Acción")]
        public string Accion { get; set; } = string.Empty;

        [Display(Name = "Entidad afectada")]
        public string EntidadAfectada { get; set; } = string.Empty;

        [Display(Name = "Fecha")]
        public DateTime Fecha { get; set; }

        [Display(Name = "Detalles")]
        public string Detalles { get; set; } = string.Empty;
    }
}
