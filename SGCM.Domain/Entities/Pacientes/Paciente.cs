using SGCM.Domain.Base;
using SGCM.Domain.Exceptions;
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

            if (string.IsNullOrEmpty(Direccion))
                throw new ExcepcionValidacion("La dirección no puede estar vacía.");
            if (Direccion.Length > 200)
                throw new ExcepcionValidacion("La dirección no puede tener más de 200 caracteres.");

            if (FechaNacimiento > DateOnly.FromDateTime(DateTime.Now))
            {
                throw new ExcepcionValidacion("La fecha de nacimiento no puede ser en el futuro.");
            }

            if (UsuarioId <= 0)
            {
                throw new ExcepcionValidacion("El ID debe ser valido.");
            }
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
