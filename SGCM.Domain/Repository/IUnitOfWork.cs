namespace SGCM.Domain.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        void RegistrarNuevo(IAggregateRoot entity, IUnitOfWorkRepository repository);
         void RegistrarAmended(IAggregateRoot entity, IUnitOfWorkRepository repository);
       void RegistrarEliminado(IAggregateRoot entity, IUnitOfWorkRepository repository);
        Task Commit(); 
    }
}
