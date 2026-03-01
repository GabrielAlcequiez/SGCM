using SGCM.Domain.Base;
using SGCM.Domain.Exceptions;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGCM.Domain.Entities.Seguridad_Usuarios
{
    [Table("Auditoria_Logs")]
    public class AuditoriaLogs : BaseEntity
    {
        public int UsuarioId { get; private set; }
        public string Accion { get; private set; }
        public string EntidadAfectada { get; private set; }
        public DateTime Fecha { get; private set; }
        public string Detalles { get; private set; }

        public AuditoriaLogs(int usuarioId, string accion, string entidadAfectada, string detalles)
        {
            UsuarioId = usuarioId;
            Accion = accion;
            EntidadAfectada = entidadAfectada;
            Detalles = detalles;
            Fecha = DateTime.Now;
            ValidarEntradaDatos();
        }

        protected override void ValidarEntradaDatos()
        {
            if (UsuarioId <= 0)
                throw new ExcepcionValidacion("El log debe estar asociado a un usuario.");
            if (EntidadAfectada.Length > 50)
                throw new ExcepcionValidacion("EntidadAfectada no puede sobrepasar los 50 caracteres");
            if(string.IsNullOrWhiteSpace(EntidadAfectada))
                throw new ExcepcionValidacion("Error, campo de entidad afectada vacio.");

            if(Accion.Length > 50)
                throw new ExcepcionValidacion("Accion no puede sobrepasar los 50 caracteres");
            if (string.IsNullOrWhiteSpace(Accion))
                throw new ExcepcionValidacion("Error, campo de acción vacio.");
            
            if(string.IsNullOrEmpty(Detalles))
                throw new ExcepcionValidacion("Error, campo de detalles vacio.");

        }
    }
}
