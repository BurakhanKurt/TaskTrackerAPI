using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskTracker.Domain.Entities;
using TaskTracker.Application.Helpers;
using TaskTracker.Core.Configuration;

namespace TaskTracker.Infrastructure.Helpers
{
    public class JwtHelper : IJwtService
    {
        private readonly string _key;
        private readonly JwtSettings _jwtSettings;

        public JwtHelper(string key, JwtSettings? jwtSettings = null) 
        { 
            _key = key;
            _jwtSettings = jwtSettings ?? new JwtSettings();
        }

        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var keyBytes = Encoding.UTF8.GetBytes(_key);
            
            // HMAC-SHA256 için minimum 256 bit (32 byte) anahtar gerekiyor
            if (keyBytes.Length < 32)
            {
                // Anahtar çok kısaysa, SHA256 ile hash'leyerek 32 byte'a genişlet
                using var sha256 = System.Security.Cryptography.SHA256.Create();
                keyBytes = sha256.ComputeHash(keyBytes);
            }
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] 
                { 
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username)
                }),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
} 