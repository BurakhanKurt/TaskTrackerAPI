using Microsoft.EntityFrameworkCore.Storage;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Domain.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        ITaskRepository TaskRepository { get; }
        IUserRepository UserRepository { get; }
        
        bool HasActiveTransaction { get; }
        
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
        
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
} 