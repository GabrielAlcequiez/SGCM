using Microsoft.EntityFrameworkCore.Storage;
using SGCM.Domain.Repository;
using SGCM.Persistence.Context;

namespace SGCM.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly SGCMContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(SGCMContext context)
    {
        _context = context;
    }

    public bool TieneTransaccionActiva => _transaction != null;

    public async Task<int> CommitAsync(CancellationToken ct = default)
    {
        return await _context.SaveChangesAsync(ct);
    }

    public async Task<int> CommitAsync(Func<Task>? postCommitAction, CancellationToken ct = default)
    {
        var result = await _context.SaveChangesAsync(ct);

        if (postCommitAction != null)
        {
            await postCommitAction();
        }

        return result;
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync(Func<Task>? postCommitAction = null)
    {
        try
        {
            await _context.SaveChangesAsync();

            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }

            if (postCommitAction != null)
            {
                await postCommitAction();
            }
        }
        catch
        {
            await RollbackTransactionAsync();
            throw;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Rollback();
        _transaction?.Dispose();
        _transaction = null;
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
