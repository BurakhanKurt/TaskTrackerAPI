using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskTracker.Application.Helpers;
using TaskTracker.Core.Configuration;
using TaskTracker.Core.Services;
using TaskTracker.Domain.Repositories;
using TaskTracker.Infrastructure.Helpers;
using TaskTracker.Infrastructure.Persistence;
using TaskTracker.Infrastructure.Persistence.Context;
using TaskTracker.Infrastructure.Persistence.Repositories;
using TaskTracker.Infrastructure.Services;

namespace TaskTracker.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            // Configure Serilog
            TaskTracker.Core.Extensions.LoggerExtensions.ConfigureLogging(config);

            // Bind configuration
            var appSettings = new AppSettings();
            config.GetSection("AppSettings").Bind(appSettings);
            services.AddSingleton(appSettings);

            // env a göre db bağlantısı
            if (appSettings.DatabaseSettings.UseInMemoryDatabase)
            {
                services.AddDbContext<TaskDbContext>(options =>
                    options.UseInMemoryDatabase("TaskDb"));
            }
            else
            {
                services.AddDbContext<TaskDbContext>(options =>
                    options.UseSqlite(appSettings.DatabaseSettings.ConnectionString));
            }

            // Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Repositories
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<IUserRepository, UserRepository>();


            services.AddSingleton<IPasswordService, PasswordHelper>();

            // User context service
            services.AddHttpContextAccessor();
            services.AddScoped<IUserContextService, UserContextService>();

            return services;
        }
    }
}
