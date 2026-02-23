namespace SGCM.Domain.Base
{
    public abstract class BaseEntity
    {
        // Solo con la propiedad ID, de forma que se alinee al SAD
        public int Id { get; set; }

        // Metodo de validación de entrada de los datos
        protected abstract void ValidarEntradaDatos();
    }
}
