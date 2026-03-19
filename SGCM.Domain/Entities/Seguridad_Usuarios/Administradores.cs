using SGCM.Domain.Base;
using SGCM.Domain.Exceptions;
using SGCM.Domain.Validaciones;

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
            ValidacionBase<Administradores>.Requerido(Nombre, "Nombre");
            ValidacionBase<Administradores>.Longitud(Nombre, 50, "Nombre");

            ValidacionBase<Administradores>.Requerido(Apellido, "Apellido");
            ValidacionBase<Administradores>.Longitud(Apellido, 50, "Apellido");

            ValidacionBase<Administradores>.Requerido(Cargo, "Cargo");
            ValidacionBase<Administradores>.Longitud(Cargo, 50, "Cargo");

            ValidacionBase<Administradores>.IdValido(UsuarioId, "UsuarioId");
        }

        public void Actualizar(string nombre, string apellido, string? cargo)
        {
            Nombre = nombre;
            Apellido = apellido;
            Cargo = cargo ?? string.Empty;
        }
    }
}
