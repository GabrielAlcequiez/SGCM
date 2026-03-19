using SGCM.Domain.Base;
using SGCM.Domain.Exceptions;
using SGCM.Domain.Validaciones;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGCM.Domain.Entities.Seguridad_Usuarios
{
    [Table("Usuarios")]
    public class Usuario : DeletableEntity
    {
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public string Rol { get; private set; }
        public DateTime FechaRegistro { get; private set; }

        protected Usuario()
        {
            
        }

        public Usuario(string email, string passwordHash, string rol)
        {
            Email = email;
            PasswordHash = passwordHash;
            Rol = rol;
            FechaRegistro = DateTime.Now;
            ValidarEntradaDatos();
        }

        protected override void ValidarEntradaDatos()
        {
            ValidacionBase<Usuario>.Requerido(Email, "Email");
            ValidacionBase<Usuario>.Longitud(Email, 100, "Email");
            ValidacionBase<Usuario>.Email(Email);

            ValidacionBase<Usuario>.Requerido(Rol, "Rol");
            ValidacionBase<Usuario>.Longitud(Rol, 20, "Rol");
        }

        public void Actualizar(string email, string rol)
        {
            Email = email;
            Rol = rol;
            ValidarEntradaDatos();
        }

        public void ActualizarPassword(string nuevoPasswordHash)
        {
            if (string.IsNullOrEmpty(nuevoPasswordHash))
                throw new ExcepcionValidacion("El password no puede estar vacío.");

            PasswordHash = nuevoPasswordHash;
        }
    }
}
