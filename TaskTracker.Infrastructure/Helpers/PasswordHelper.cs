using System.Security.Cryptography;
using System.Text;
using TaskTracker.Application.Helpers;

namespace TaskTracker.Infrastructure.Helpers
{
    /// <summary>
    /// Şifre işlemleri için yardımcı sınıf
    /// </summary>
    public class PasswordHelper : IPasswordService
    {
        /// <summary>
        /// Şifre hash'i oluşturur
        /// </summary>
        /// <param name="password">Şifre</param>
        /// <param name="salt">Salt</param>
        /// <returns>Hash'lenmiş şifre</returns>
        public string HashPassword(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var passwordWithSalt = password + salt;
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(passwordWithSalt));
                return Convert.ToBase64String(bytes);
            }
        }

        /// <summary>
        /// Rastgele salt oluşturur
        /// </summary>
        /// <returns>Salt string</returns>
        private string GenerateSalt()
        {
            var randomBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }
            return Convert.ToBase64String(randomBytes);
        }

        /// <summary>
        /// Şifre doğrulaması yapar
        /// </summary>
        /// <param name="password">Girilen şifre</param>
        /// <param name="hash">Kayıtlı hash</param>
        /// <param name="salt">Kayıtlı salt</param>
        /// <returns>Doğrulama sonucu</returns>
        public bool VerifyPassword(string password, string hash, string salt)
        {
            var computedHash = HashPassword(password, salt);
            return computedHash == hash;
        }

        /// <summary>
        /// Şifre ve salt oluşturur
        /// </summary>
        /// <param name="password">Şifre</param>
        /// <returns>(Hash, Salt) tuple</returns>
        public (string Hash, string Salt) HashPassword(string password)
        {
            var salt = GenerateSalt();
            var hash = HashPassword(password, salt);
            return (hash, salt);
        }
    }
} 