namespace SGCM.Domain.Repository
{
    public interface IUnitOfWorkRepository
    {
        void PersistirCreacion(IAggregateRoot entity);
        void PersistirModificacion(IAggregateRoot entity);
        void PersistirEliminacion(IAggregateRoot entity);
    }
}
