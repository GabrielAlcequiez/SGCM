namespace SGCM.Domain.Repository;

public interface IUnitOfWork : IDisposable
{
    Task<int> CommitAsync(CancellationToken ct = default);
    Task<int> CommitAsync(Func<Task>? postCommitAction, CancellationToken ct = default);
    Task BeginTransactionAsync();
    Task CommitTransactionAsync(Func<Task>? postCommitAction = null);
    Task RollbackTransactionAsync();
    bool TieneTransaccionActiva { get; }
}
