using SGCM.Domain.Base;
using SGCM.Domain.Exceptions;
using SGCM.Domain.Validaciones;
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

        protected AuditoriaLogs() { }

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
            ValidacionBase<AuditoriaLogs>.IdValido(UsuarioId, "UsuarioId");

            ValidacionBase<AuditoriaLogs>.Requerido(EntidadAfectada, "EntidadAfectada");
            ValidacionBase<AuditoriaLogs>.Longitud(EntidadAfectada, 50, "EntidadAfectada");

            ValidacionBase<AuditoriaLogs>.Requerido(Accion, "Accion");
            ValidacionBase<AuditoriaLogs>.Longitud(Accion, 50, "Accion");

            ValidacionBase<AuditoriaLogs>.Requerido(Detalles, "Detalles");
        }
    }
}
