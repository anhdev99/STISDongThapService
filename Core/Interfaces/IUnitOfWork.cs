using Microsoft.EntityFrameworkCore.Storage;
using Shared;

namespace Core.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<T> Repository<T>() where T : BaseAuditableEntity;

    Task<int> Save(CancellationToken cancellationToken);
    Task<int> SaveAndRemoveCache(CancellationToken cancellationToken, params string[] cacheKeys);
    Task Rollback();
    
    Task<IDbContextTransaction> BeginTransactionAsync();

}