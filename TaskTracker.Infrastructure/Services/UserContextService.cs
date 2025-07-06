using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TaskTracker.Core.Exceptions;
using TaskTracker.Core.Services;

namespace TaskTracker.Infrastructure.Services
{
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetCurrentUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
            throw new UnauthorizedException("Kullanýcý kimliði doðrulanamadý");
        }

        public string? GetCurrentUserEmail()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
        }

        public string? GetCurrentUsername()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;
        }

        public bool IsAuthenticated()
        {
            return _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
        }
    }
} 