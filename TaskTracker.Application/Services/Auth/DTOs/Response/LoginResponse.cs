namespace TaskTracker.Application.Services.Auth.DTOs.Response
{
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
} 