using TaskTracker.Core.Entity;

namespace TaskTracker.Domain.Entities
{
    /// <summary>
    /// Kullanıcı entity'si
    /// </summary>
    public class User : BaseEntity
    {
        /// <summary>
        /// Kullanıcı adı (benzersiz)
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Email adresi (benzersiz)
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Şifre hash'i
        /// </summary>
        public string PasswordHash { get; set; } = string.Empty;

        /// <summary>
        /// Şifre salt'ı
        /// </summary>
        public string PasswordSalt { get; set; } = string.Empty;

        /// <summary>
        /// Ad
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// Soyad
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Telefon numarası
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Son giriş tarihi
        /// </summary>
        public DateTime? LastLoginAt { get; set; }

        /// <summary>
        /// Tam ad (computed property)
        /// </summary>
        public string? FullName => $"{FirstName} {LastName}".Trim();
    }
} 