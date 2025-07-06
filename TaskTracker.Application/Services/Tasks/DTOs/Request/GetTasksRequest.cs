namespace TaskTracker.Application.Services.Tasks.DTOs.Request
{
    public class GetTasksRequest
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        
        // Filtreleme parametreleri
        public TaskStatusFilter? StatusFilter { get; set; }
        public string? SearchTerm { get; set; }
        public DateOnly? DueDate { get; set; }
    }

    public enum TaskStatusFilter
    {
        All = 0,
        Completed = 1,
        Pending = 2
    }
} 