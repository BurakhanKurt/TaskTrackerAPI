using TaskTracker.Core.Configuration;

namespace TaskTracker.API.Middleware
{
    public static class CorsMiddleware
    {
        public static IServiceCollection AddCorsPolicy(this IServiceCollection services, CorsSettings settings)
        {
            //daha sonra settings �zerinden kontrol edilebilir
            services.AddCors(options =>
            {
                options.AddPolicy("TaskTrackerCorsPolicy", policy =>
                {
                    policy
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            return services;
        }
    }
} 