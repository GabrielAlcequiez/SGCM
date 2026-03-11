namespace SGCM.Domain.Base
{
    public abstract class DeletableEntity : BaseEntity
    {
        public bool EstaEliminado { get; protected set; } = false;

        public virtual void Eliminar()
        {
            EstaEliminado = true;
        }

    }
}
