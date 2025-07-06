using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskTracker.Core.Extensions;
using TaskTracker.Core.Exceptions;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Repositories;
using TaskTracker.Infrastructure.Persistence.Context;

namespace TaskTracker.Infrastructure.Persistence.Repositories
{
    public class TaskRepository : BaseRepository<TaskRepository>, ITaskRepository
    {
        public TaskRepository(TaskDbContext context, ILogger<TaskRepository> logger) : base(context, logger)
        {
        }

        public async Task<int> GetTotalCountByUserIdAsync(int userId, CancellationToken cancellationToken = default)
        {
            return await GetQueryable<TaskItem>()
                .Where(x => x.UserId == userId)
                .CountAsync(cancellationToken);
        }

        public async Task<int> GetCompletedCountByUserIdAsync(int userId, CancellationToken cancellationToken = default)
        {
            return await GetQueryable<TaskItem>()
                .Where(x => x.UserId == userId && x.IsCompleted)
                .CountAsync(cancellationToken);
        }

        public async Task<int> GetPendingCountByUserIdAsync(int userId, CancellationToken cancellationToken = default)
        {
            return await GetQueryable<TaskItem>()
                .Where(x => x.UserId == userId && !x.IsCompleted)
                .CountAsync(cancellationToken);
        }

        public async Task<TaskItem?> GetByIdAndUserIdAsync(int id, int userId, CancellationToken cancellationToken = default)
        {
            return await GetQueryable<TaskItem>()
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId, cancellationToken);
        }

        public async Task<TaskItem> AddAsync(TaskItem task, CancellationToken cancellationToken = default)
        {
            _context.Tasks.Add(task);
            await SaveChangesAsync(cancellationToken);
            return task;
        }

        public async Task UpdateAsync(TaskItem task, CancellationToken cancellationToken = default)
        {
            _context.Tasks.Update(task);
            await SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(TaskItem task, int userId, CancellationToken cancellationToken = default)
        {
            _context.Tasks.Remove(task);
            await SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<TaskItem>> GetFilteredTasksAsync(
            int userId, 
            int page, 
            int pageSize, 
            bool? isCompleted = null,
            string? searchTerm = null,
            DateOnly? dueDate = null,
            CancellationToken cancellationToken = default)
        {
            var query = GetQueryable<TaskItem>().Where(x => x.UserId == userId);

            if (isCompleted.HasValue)
            {
                query = query.Where(x => x.IsCompleted == isCompleted.Value);
            }
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var normalizedSearchTerm = searchTerm.ToLower().Replace(" ", "");
                query = query.Where(x => x.NormalizedTitle != null && x.NormalizedTitle.Contains(normalizedSearchTerm));
            }
            if (dueDate.HasValue)
            {
                var searchDate = dueDate.Value.ToDateTime(new TimeOnly(0, 0));
                query = query.Where(x => x.DueDate.HasValue && x.DueDate.Value.Date == searchDate.Date);
            }

            var tasks = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return tasks;
        }

        public async Task<int> GetFilteredTasksCountAsync(
            int userId,
            bool? isCompleted = null,
            string? searchTerm = null,
            DateOnly? dueDate = null,
            CancellationToken cancellationToken = default)
        {
            var query = GetQueryable<TaskItem>().Where(x => x.UserId == userId);

            if (isCompleted.HasValue)
            {
                query = query.Where(x => x.IsCompleted == isCompleted.Value);
            }
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(x => x.Title.Contains(searchTerm));
            }
            if (dueDate.HasValue)
            {
                var searchDate = dueDate.Value.ToDateTime(new TimeOnly(0, 0));
                query = query.Where(x => x.DueDate.HasValue && x.DueDate.Value.Date == searchDate.Date);
            }

            return await query.CountAsync(cancellationToken);
        }
    }
} 