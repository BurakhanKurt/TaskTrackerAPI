using TaskTracker.Core.Entity;

namespace TaskTracker.Domain.Entities
{
    /// <summary>
    /// Görev entity'si
    /// </summary>
    public class TaskItem : BaseEntity
    {
        /// <summary>
        /// Görevin başlığı
        /// </summary>
        public string? Title { get; private set; }

        public string? NormalizedTitle { get; set; }

        public void SetTitle(string title)
        {
            Title = title;
            NormalizedTitle = title?.ToLower().Replace(" ", "");
        }

        public void SetDueDate(DateTime? dueDate)
        {
            DueDate = dueDate;
        }

        /// <summary>
        /// Görevin tamamlanma durumu
        /// </summary>
        public bool IsCompleted { get; set; } = false;

        /// <summary>
        /// Görevin oluşturulma tarihi
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Görevin bitiş tarihi
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Görevi oluşturan kullanıcı ID'si
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Görevi oluşturan kullanıcı (navigation property)
        /// </summary>
        public virtual User? User { get; set; }

        /// <summary>
        /// Tamamlanma tarihi
        /// </summary>
        public DateTime? CompletedAt { get; set; }
    }
} 