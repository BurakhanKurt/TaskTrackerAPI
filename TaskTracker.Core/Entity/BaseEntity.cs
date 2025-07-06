using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker.Core.Entity
{
    /// <summary>
    /// Tüm entity'ler için temel sınıf
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Benzersiz kimlik
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Oluşturulma tarihi
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Güncellenme tarihi
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Silinme durumu (soft delete)
        /// </summary>
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// Silinme tarihi
        /// </summary>
        public DateTime? DeletedAt { get; set; }
    }
}
