using SGCM.Domain.Base;
using SGCM.Domain.Exceptions;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGCM.Domain.Entities.Medicos
{
    [Table("Medicos")]
    public class Medico : BaseEntity, IAggregateRoot
    {
        public string Nombre { get; private set; }
        public string Apellido { get; private set; }
        public string Exequatur { get; private set; }
        public string Telefono { get; private set; }
        public int EspecialidadId { get; private set; }
        public int UsuarioId { get; private set; }

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
            if (string.IsNullOrEmpty(Nombre))
                throw new ExcepcionValidacion("El nombre no puede estar vacío.");
            if (Nombre.Length > 50)
                throw new ExcepcionValidacion("El nombre no puede tener más de 50 caracteres.");

            if (string.IsNullOrEmpty(Apellido))
                throw new ExcepcionValidacion("El apellido no puede estar vacío.");
            if (Apellido.Length > 50)
                throw new ExcepcionValidacion("El apellido no puede tener más de 50 caracteres.");

            if (string.IsNullOrEmpty(Telefono))
                throw new ExcepcionValidacion("El teléfono no puede estar vacío.");
            if (Telefono.Length > 20)
                throw new ExcepcionValidacion("El teléfono no puede tener más de 20 caracteres.");

            if (string.IsNullOrEmpty(Exequatur))
                throw new ExcepcionValidacion("El exequatur no puede estar vacío.");
            if (Exequatur.Length > 20)
                throw new ExcepcionValidacion("El exequatur no puede tener más de 20 caracteres.");
        }
    }
}
