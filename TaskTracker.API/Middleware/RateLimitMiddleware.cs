using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using TaskTracker.Core.Configuration;
using TaskTracker.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace TaskTracker.API.Middleware
{
    public static class RateLimitMiddleware
    {
        public static IServiceCollection AddRateLimiting(this IServiceCollection services, RateLimitSettings settings)
        {
            services.AddRateLimiter(options =>
            {
                // Global rate limiting
                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
                {
                    // TODO: Geçici olarak UserId tabanlı rate limiting kullanıldı.
                    // İleride ip tabanlı sisteme geçiş yapılabilir x-real-ıp gibi,
                    //kurum içi aynı network kullanılıyorsa farklı bir yola gidilebilir
                    //var clientId = context.Connection.RemoteIpAddress?.ToString() ?? "anonymous";
                    
                    var userContextService = context.RequestServices.GetRequiredService<IUserContextService>();
                    var clientId = userContextService.IsAuthenticated() 
                        ? $"user_{userContextService.GetCurrentUserId()}" 
                        : "anonymous";
                    
                    return RateLimitPartition.GetFixedWindowLimiter(clientId, _ =>
                        new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = settings.PermitLimit,
                            Window = TimeSpan.FromSeconds(settings.Window),
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = settings.QueueLimit
                        });
                });

                // Authentication endpoint'leri için
                options.AddPolicy("auth", context =>
                {
                    // TODO: Kurum içi rate limiting için farklı bir sistem kullanılabilir
                    var clientId = context.Connection.RemoteIpAddress?.ToString() ?? "anonymous";
                    
                    return RateLimitPartition.GetTokenBucketLimiter(clientId, _ =>
                        new TokenBucketRateLimiterOptions
                        {
                            TokenLimit = settings.TokenLimit,
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = settings.QueueLimit,
                            ReplenishmentPeriod = TimeSpan.FromSeconds(settings.ReplenishmentPeriod),
                            TokensPerPeriod = settings.TokensPerPeriod
                        });
                });

                // Rate limit aşılırsa
                options.OnRejected = async (context, token) =>
                {
                    context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    context.HttpContext.Response.ContentType = "application/json";
                    
                    var response = new
                    {
                        isSuccessful = false,
                        messages = "Çok fazla sayıda istek attın. Sen ne yapmaya çalışıyorsun? :)",
                        retryAfter = context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter) 
                            ? retryAfter.TotalSeconds 
                            : 60
                    };

                    await context.HttpContext.Response.WriteAsJsonAsync(response, token);
                };
            });

            return services;
        }
    }
} 