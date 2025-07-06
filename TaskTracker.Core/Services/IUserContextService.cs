namespace TaskTracker.Core.Services
{
    public interface IUserContextService
    {
        int GetCurrentUserId();
        string? GetCurrentUserEmail();
        string? GetCurrentUsername();
        bool IsAuthenticated();
    }
} 