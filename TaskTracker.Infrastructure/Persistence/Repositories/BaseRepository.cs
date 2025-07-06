using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using TaskTracker.Core.Extensions;
using TaskTracker.Infrastructure.Persistence.Context;

namespace TaskTracker.Infrastructure.Persistence.Repositories
{
    public abstract class BaseRepository<TLogger> : IDisposable
    {
        protected readonly TaskDbContext _context;
        protected readonly ILogger<TLogger> _logger;
        private bool _disposed = false;

        protected BaseRepository(TaskDbContext context, ILogger<TLogger> logger)
        {
            _context = context;
            _logger = logger;
        }

        protected IQueryable<T> GetQueryable<T>(bool asNoTracking = true) where T : class
        {
            var query = _context.Set<T>().AsQueryable();
            
            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }
            
            return query;
        }

        protected async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _context.SaveChangesAsync(cancellationToken);
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.SendError($"SaveChanges failed", ex);
                throw;
            }
        }

        protected async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Database.BeginTransactionAsync(cancellationToken);
        }

        protected async Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> operation, string operationName = "", CancellationToken cancellationToken = default)
        {
            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var result = await operation();
                await transaction.CommitAsync(cancellationToken);
                _logger.LogInformation($"Transaction completed successfully for operation: {operationName}");
                return result;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                _logger.SendError($"Transaction failed for operation: {operationName}", ex);
                throw;
            }
        }

        protected async Task ExecuteInTransactionAsync(Func<Task> operation, string operationName = "", CancellationToken cancellationToken = default)
        {
            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                await operation();
                await transaction.CommitAsync(cancellationToken);
                _logger.LogInformation($"Transaction completed successfully for operation: {operationName}");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                _logger.SendError($"Transaction failed for operation: {operationName}", ex);
                throw;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
} 