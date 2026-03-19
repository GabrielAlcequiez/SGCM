using SGCM.Domain.Base;
using SGCM.Domain.Exceptions;
using SGCM.Domain.Validaciones;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGCM.Domain.Entities.Pacientes
{
    [Table("Pacientes")]
    public class Paciente : DeletableEntity
    {
        public string Nombre { get; private set; }
        public string Apellido { get; private set; }
        public string Telefono { get; private set; }
        public string Direccion { get; private set; }
        public DateOnly FechaNacimiento { get; private set; }
            
        public int? ProveedorId{ get; private set; }
        public string? NSS { get; private set; }
        public int UsuarioId { get; private set; }

        protected Paciente()
        {
            
        }
        public Paciente(string nombre, string apellido, string telefono, string direccion, DateOnly fechaNacimiento, int? proveedorId, string? nss, int usuarioid)
        {
            Nombre = nombre;
            Apellido = apellido;
            Telefono = telefono;
            Direccion = direccion;
            FechaNacimiento = fechaNacimiento;
            ProveedorId = proveedorId;
            NSS = nss;
            UsuarioId = usuarioid;
            ValidarEntradaDatos();
        }

        protected override void ValidarEntradaDatos()
        {
            ValidacionBase<Paciente>.Requerido(Nombre, "Nombre");
            ValidacionBase<Paciente>.Longitud(Nombre, 50, "Nombre");

            ValidacionBase<Paciente>.Requerido(Apellido, "Apellido");
            ValidacionBase<Paciente>.Longitud(Apellido, 50, "Apellido");

            ValidacionBase<Paciente>.Requerido(Telefono, "Telefono");
            ValidacionBase<Paciente>.Longitud(Telefono, 20, "Telefono");
            ValidacionBase<Paciente>.Telefono(Telefono);

            ValidacionBase<Paciente>.Requerido(Direccion, "Direccion");
            ValidacionBase<Paciente>.Longitud(Direccion, 200, "Direccion");

            ValidacionBase<Paciente>.FechaNoFutura(FechaNacimiento, "Fecha de nacimiento");

            ValidacionBase<Paciente>.IdValido(UsuarioId, "UsuarioId");
        }

        public void Actualizar(string nombre, string apellido, string telefono,
                       string direccion, DateOnly fechaNacimiento,
                       int? proveedorId, string? nss)
        {
            Nombre = nombre;
            Apellido = apellido;
            Telefono = telefono;
            Direccion = direccion;
            FechaNacimiento = fechaNacimiento;
            ProveedorId = proveedorId;
            NSS = nss;
            ValidarEntradaDatos();
        }
    }
}
