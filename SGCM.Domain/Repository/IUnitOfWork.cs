namespace SGCM.Domain.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        Task CommitAsync();
    }
}
