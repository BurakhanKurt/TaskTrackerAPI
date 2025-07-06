namespace TaskTracker.Application.Services.Tasks.DTOs.Response
{
    public class TaskItemDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public DateTime? DueDate { get; set; }
    }
} 