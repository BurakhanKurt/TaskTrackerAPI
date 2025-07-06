using TaskTracker.Domain.Entities;

namespace TaskTracker.Domain.Repositories
{
    public interface ITaskRepository
    {
        Task<int> GetTotalCountByUserIdAsync(int userId, CancellationToken cancellationToken = default);
        Task<int> GetCompletedCountByUserIdAsync(int userId, CancellationToken cancellationToken = default);
        Task<int> GetPendingCountByUserIdAsync(int userId, CancellationToken cancellationToken = default);
        Task<TaskItem?> GetByIdAndUserIdAsync(int id, int userId, CancellationToken cancellationToken = default);
        Task<TaskItem> AddAsync(TaskItem task, CancellationToken cancellationToken = default);
        Task UpdateAsync(TaskItem task, CancellationToken cancellationToken = default);
        Task DeleteAsync(TaskItem task, int userId, CancellationToken cancellationToken = default);
        
        // Yeni arama ve filtreleme metodları
        Task<IEnumerable<TaskItem>> GetFilteredTasksAsync(
            int userId, 
            int page, 
            int pageSize, 
            bool? isCompleted = null,
            string? searchTerm = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            CancellationToken cancellationToken = default);
            
        Task<int> GetFilteredTasksCountAsync(
            int userId,
            bool? isCompleted = null,
            string? searchTerm = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            CancellationToken cancellationToken = default);
    }
} 