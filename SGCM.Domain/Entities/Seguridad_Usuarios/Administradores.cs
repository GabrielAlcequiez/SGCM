using SGCM.Domain.Base;
using SGCM.Domain.Exceptions;
using SGCM.Domain.Repository;

namespace SGCM.Domain.Entities.Seguridad_Usuarios
{
    public class Administradores : BaseEntity, IAggregateRoot
    {
        public string Nombre { get; private set; }
        public string Apellido { get; private set; }

        public int UsuarioId { get; private set; }
        // Para decisión de diferentes tiposd de admin, como la secretaria, los jefes, etc.
        //public string Cargo { get; private set; } 

        public Administradores(string nombre, string apellido, int usuarioId)
        {
            Nombre = nombre;
            Apellido = apellido;
            UsuarioId = usuarioId;
            ValidarEntradaDatos();

        }

        protected override void ValidarEntradaDatos()
        {
            if (Nombre.Length > 50)
                throw new ExcepcionValidacion("El nombre no puede tener más de 50 caracteres.");
            if (string.IsNullOrWhiteSpace(Nombre))
                throw new ExcepcionValidacion("El nombre no puede estar vacío.");

            if (string.IsNullOrWhiteSpace(Apellido))
                throw new ExcepcionValidacion("El apellido no puede estar vacío.");
            if (Apellido.Length > 50)
                throw new ExcepcionValidacion("El apellido no puede tener más de 50 caracteres.");

            if (UsuarioId <= 0)
                throw new ExcepcionValidacion("El ID debe ser valido.");
        }
    }
}
