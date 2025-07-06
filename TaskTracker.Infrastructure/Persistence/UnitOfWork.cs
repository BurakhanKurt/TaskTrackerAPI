using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using TaskTracker.Core.Extensions;
using TaskTracker.Domain.Repositories;
using TaskTracker.Infrastructure.Persistence.Context;
using TaskTracker.Infrastructure.Persistence.Repositories;

namespace TaskTracker.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TaskDbContext _context;
        private readonly ILogger<UnitOfWork> _logger;
        private readonly ILoggerFactory _loggerFactory;
        private IDbContextTransaction? _currentTransaction;

        public UnitOfWork(TaskDbContext context, ILogger<UnitOfWork> logger, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = logger;
            _loggerFactory = loggerFactory;
        }

        public ITaskRepository TaskRepository => new TaskRepository(_context, _loggerFactory.CreateLogger<TaskRepository>());
        public IUserRepository UserRepository => new UserRepository(_context, _loggerFactory.CreateLogger<UserRepository>());

        public bool HasActiveTransaction => _currentTransaction != null;

        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction != null)
            {
                return _currentTransaction;
            }

            _currentTransaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            _logger.LogInformation("Transaction started");
            return _currentTransaction;
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                if (_currentTransaction != null)
                {
                    await _currentTransaction.CommitAsync(cancellationToken);
                    _logger.LogInformation("Transaction committed successfully");
                }
            }
            catch (Exception ex)
            {
                _logger.SendError("Failed to commit transaction", ex);
                await RollbackTransactionAsync(cancellationToken);
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                if (_currentTransaction != null)
                {
                    await _currentTransaction.RollbackAsync(cancellationToken);
                    _logger.LogInformation("Transaction rolled back");
                }
            }
            catch (Exception ex)
            {
                _logger.SendError("Failed to rollback transaction", ex);
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _context.SaveChangesAsync(cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                _logger.SendError("SaveChanges failed in UnitOfWork", ex);
                throw;
            }
        }

        public void Dispose()
        {
            _currentTransaction?.Dispose();
        }
    }
} 