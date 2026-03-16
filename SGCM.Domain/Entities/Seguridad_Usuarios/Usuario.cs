using SGCM.Domain.Base;
using SGCM.Domain.Exceptions;
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
            if (string.IsNullOrEmpty(Email) || !Email.Contains("@"))
                throw new ExcepcionValidacion("Email inválido.");
            if (Email.Length > 100)
                throw new ExcepcionValidacion("El email no puede tener más de 100 caracteres.");

            if (string.IsNullOrEmpty(Rol))
                throw new ExcepcionValidacion("El rol es obligatorio.");
            if (Rol.Length > 20)
                throw new ExcepcionValidacion("El rol no puede tener más de 20 caracteres.");
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
