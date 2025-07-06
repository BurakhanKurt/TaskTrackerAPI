namespace TaskTracker.Core.Configuration
{
    public class AppSettings
    {
        public JwtSettings JwtSettings { get; set; } = new();
        public DatabaseSettings DatabaseSettings { get; set; } = new();
        public LoggingSettings LoggingSettings { get; set; } = new();
        public RateLimitSettings RateLimitSettings { get; set; } = new();
        public CorsSettings CorsSettings { get; set; } = new();
    }

    public class JwtSettings
    {
        public string SecretKey { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int ExpirationInMinutes { get; set; } = 60;
    }

    public class DatabaseSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public bool UseInMemoryDatabase { get; set; } = false;
    }

    public class LoggingSettings
    {
        public string LogLevel { get; set; } = "Information";
        public string LogFilePath { get; set; } = "logs/tasktracker-.txt";
        public int RetainedFileCountLimit { get; set; } = 30;
    }

    public class RateLimitSettings
    {
        public int PermitLimit { get; set; } = 100;
        public int Window { get; set; } = 60;
        public int SegmentsPerWindow { get; set; } = 1;
        public int QueueLimit { get; set; } = 2;
        public int TokenLimit { get; set; } = 10;
        public int TokensPerPeriod { get; set; } = 10;
        public int ReplenishmentPeriod { get; set; } = 1;
    }

    public class CorsSettings
    {
        public string[] AllowedOrigins { get; set; } = new[] { "http://localhost:3000" };
        public string[] AllowedMethods { get; set; } = new[] { "GET", "POST", "PUT", "DELETE", "PATCH", "OPTIONS" };
        public string[] AllowedHeaders { get; set; } = new[] { "Content-Type", "Authorization", "X-Requested-With", "Accept", "Origin" };
        public bool AllowCredentials { get; set; } = true;
    }
} 