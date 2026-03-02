using SGCM.Domain.Base;
using SGCM.Domain.Exceptions;
using SGCM.Domain.Repository;

namespace SGCM.Domain.Entities.Pacientes
{
    public class Proveedores : BaseEntity, IAggregateRoot
    {
        public string Nombre { get; private set; }
        public string RNC { get; private set; }
        public string Telefono { get; private set; }
        public decimal CoberturaDefault { get; private set; }

        public Proveedores(string nombre, string rnc, string telefono, decimal coberturaDefault)
        {

            Nombre = nombre;
            RNC = rnc;
            Telefono = telefono;
            CoberturaDefault = coberturaDefault;
            ValidarEntradaDatos();

        }

        protected override void ValidarEntradaDatos()
        {
            if (string.IsNullOrWhiteSpace(Nombre))
                throw new ExcepcionValidacion("El nombre del proveedor es obligatorio.");
            if (Nombre.Length > 100)
                throw new ExcepcionValidacion("El nombre del proveedor no puede tener más de 100 caracteres.");

            if (string.IsNullOrWhiteSpace(RNC))
                throw new ExcepcionValidacion("El RNC del proveedor es obligatorio");
            if(RNC.Length > 20)
                throw new ExcepcionValidacion("El RNC del proveedor no puede tener más de 20 caracteres.");

            if (string.IsNullOrWhiteSpace(Telefono))
                throw new ExcepcionValidacion("El teléfono del proveedor es obligatorio");
            if(Telefono.Length > 20)
                throw new ExcepcionValidacion("El teléfono del proveedor no puede tener más de 20 caracteres.");

            if (CoberturaDefault < 0 || CoberturaDefault > 100)
                throw new ExcepcionValidacion("La cobertura default debe estar entre 0 y 100.");
        }
    }
}
