namespace TaskTracker.Application.Services.Tasks.DTOs.Response
{
    public class GetTasksResponse
    {
        public IEnumerable<TaskItemDto>? Tasks { get; set; }
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        
        // Summary
        public int TotalTasks { get; set; }
        public int Completed { get; set; }
        public int Pending { get; set; }
        public int Progress { get; set; }
    }
}
