using TaskTracker.Domain.Entities;

namespace TaskTracker.Application.Helpers
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
} 