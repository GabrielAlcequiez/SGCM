using SGCM.Domain.Base;
using SGCM.Domain.Exceptions;

namespace SGCM.Domain.Entities.Seguridad_Usuarios
{
    public class Administradores : DeletableEntity
    {
        public string Nombre { get; private set; }
        public string Apellido { get; private set; }

        public int UsuarioId { get; private set; }

        public string Cargo { get; private set; }

        protected Administradores() { }

        public Administradores(int usuarioId, string nombre, string apellido, string cargo)
        {
            UsuarioId = usuarioId;
            Nombre = nombre;
            Apellido = apellido;
            Cargo = cargo;
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

            if (Cargo.Length > 50)
            {
                throw new ExcepcionValidacion("El cargo no puede tener más de 50 caracteres.");
            }

            if (UsuarioId < 0)
                throw new ExcepcionValidacion("El ID debe ser valido.");

        }

        public void Actualizar(string nombre, string apellido, string? cargo)
        {
            Nombre = nombre;
            Apellido = apellido;
            Cargo = cargo ?? string.Empty;
        }
    }
}
