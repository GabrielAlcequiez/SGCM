using SGCM.Domain.Base;
using SGCM.Domain.Exceptions;
using SGCM.Domain.Validaciones;

namespace SGCM.Domain.Entities.Medicos
{
    public class Especialidades : DeletableEntity
    {
        public string Nombre { get; private set; }
        public string Descripcion { get; private set; }

        protected Especialidades() { }

        public Especialidades(string nombre, string descripcion)
        {
            Nombre = nombre;
            Descripcion = descripcion;
            ValidarEntradaDatos();
        }

        protected override void ValidarEntradaDatos()
        {
            ValidacionBase<Especialidades>.Requerido(Nombre, "Nombre");
            ValidacionBase<Especialidades>.Longitud(Nombre, 50, "Nombre");

            ValidacionBase<Especialidades>.Requerido(Descripcion, "Descripcion");
            ValidacionBase<Especialidades>.Longitud(Descripcion, 200, "Descripcion");
        }

        public void Actualizar(string nombre, string descripcion)
        {
            Nombre = nombre;
            Descripcion = descripcion;
            ValidarEntradaDatos();
        }
    }
}
