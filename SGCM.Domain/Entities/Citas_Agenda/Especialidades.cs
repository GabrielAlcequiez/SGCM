using SGCM.Domain.Base;
using SGCM.Domain.Exceptions;

namespace SGCM.Domain.Entities.Medicos
{
    public class Especialidades : DeletableEntity
    {
        public string Nombre { get; private set; }
        public string Descripcion { get; private set; }

        public Especialidades(string nombre, string descripcion)
        {
            Nombre = nombre;
            Descripcion = descripcion;
            ValidarEntradaDatos();
        }

        protected override void ValidarEntradaDatos()
        {
            if (string.IsNullOrEmpty(Nombre))
                throw new ExcepcionValidacion("El nombre de la especialidad no puede estar vacío.");
            if (Nombre.Length > 50
                ) throw new ExcepcionValidacion("El nombre de la especialidad no debe sobrepasar los 50 caracteres");
            if (string.IsNullOrEmpty(Descripcion))
                throw new ExcepcionValidacion("La descripción no puede estar vacia");
            if (Descripcion.Length > 200)
                throw new ExcepcionValidacion("La descripción no debe sobrepasar los 200 caracteres");
        }

        public void Actualizar(string nombre, string descripcion)
        {
            Nombre = nombre;
            Descripcion = descripcion;
            ValidarEntradaDatos();
        }
    }
}
