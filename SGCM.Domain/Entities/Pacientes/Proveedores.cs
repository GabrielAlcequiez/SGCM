using SGCM.Domain.Base;
using SGCM.Domain.Exceptions;
using SGCM.Domain.Validaciones;

namespace SGCM.Domain.Entities.Pacientes
{
    public class Proveedores : DeletableEntity
    {
        public string Nombre { get; private set; }
        public string RNC { get; private set; }
        public string Telefono { get; private set; }
        public decimal CoberturaDefault { get; private set; }

        protected Proveedores() { }

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
            ValidacionBase<Proveedores>.Requerido(Nombre, "Nombre");
            ValidacionBase<Proveedores>.Longitud(Nombre, 100, "Nombre");

            ValidacionBase<Proveedores>.Requerido(RNC, "RNC");
            ValidacionBase<Proveedores>.Longitud(RNC, 20, "RNC");
            ValidacionBase<Proveedores>.RNC(RNC);

            ValidacionBase<Proveedores>.Requerido(Telefono, "Telefono");
            ValidacionBase<Proveedores>.Longitud(Telefono, 20, "Telefono");
            ValidacionBase<Proveedores>.Telefono(Telefono);

            if (CoberturaDefault < 0 || CoberturaDefault > 100)
                throw new ExcepcionValidacion("La cobertura default debe estar entre 0 y 100.");
        }

        public void Actualizar(string nombre, string rnc, string telefono, decimal coberturaDefault)
        {
            Nombre = nombre;
            RNC = rnc;
            Telefono = telefono;
            CoberturaDefault = coberturaDefault;
            ValidarEntradaDatos();
        }
    }
}
