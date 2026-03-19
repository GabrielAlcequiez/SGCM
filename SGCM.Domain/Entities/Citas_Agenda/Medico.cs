using SGCM.Domain.Base;
using SGCM.Domain.Exceptions;
using SGCM.Domain.Validaciones;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGCM.Domain.Entities.Medicos
{
    [Table("Medicos")]
    public class Medico : DeletableEntity
    {
        public string Nombre { get; private set; }
        public string Apellido { get; private set; }
        public string Exequatur { get; private set; }
        public string Telefono { get; private set; }
        public int EspecialidadId { get; private set; }
        public int UsuarioId { get; private set; }

        protected Medico() { }

        public Medico(string nombre, string apellido, string exequatur, string telefono, int especialidadId, int usuarioId)
        {
            Nombre = nombre;
            Apellido = apellido;
            Exequatur = exequatur;
            Telefono = telefono;
            EspecialidadId = especialidadId;
            UsuarioId = usuarioId;
            ValidarEntradaDatos();
        }

        protected override void ValidarEntradaDatos()
        {
            ValidacionBase<Medico>.Requerido(Nombre, "Nombre");
            ValidacionBase<Medico>.Longitud(Nombre, 50, "Nombre");

            ValidacionBase<Medico>.Requerido(Apellido, "Apellido");
            ValidacionBase<Medico>.Longitud(Apellido, 50, "Apellido");

            ValidacionBase<Medico>.Requerido(Telefono, "Telefono");
            ValidacionBase<Medico>.Longitud(Telefono, 20, "Telefono");
            ValidacionBase<Medico>.Telefono(Telefono);

            ValidacionBase<Medico>.Requerido(Exequatur, "Exequatur");
            ValidacionBase<Medico>.Longitud(Exequatur, 20, "Exequatur");
        }

        public void Actualizar(string nombre, string apellido, string exequatur, string telefono, int especialidadId)
        {
            Nombre = nombre;
            Apellido = apellido;
            Exequatur = exequatur;
            Telefono = telefono;
            EspecialidadId = especialidadId;
            ValidarEntradaDatos();
        }
    }
}
