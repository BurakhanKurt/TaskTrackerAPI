using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Runtime.CompilerServices;

namespace TaskTracker.Core.Extensions
{
    public static class LoggerExtensions
    {
        public static void ConfigureLogging(IConfiguration configuration)
        {
            try
            {
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                var loggerConfig = new LoggerConfiguration()
                    .MinimumLevel.Error()
                    .Enrich.FromLogContext()
                    .Enrich.WithMachineName()
                    .Enrich.WithProperty("Environment", environment)
                    .WriteTo.Console()
                    .WriteTo.File("logs/tasktracker-.txt", rollingInterval: Serilog.RollingInterval.Day, retainedFileCountLimit: 30);

                Log.Logger = loggerConfig.CreateLogger();

                Log.Information("TaskTracker Logger started successfully");
            }
            catch (Exception ex)
            {
                Log.Error("Logger Configuration Exception: " + ex.Message);
            }
        }

        public static void SendError<T>(this ILogger<T> logger, Exception ex, [CallerMemberName] string methodName = "")
        {
            logger.LogError(ex, "{MethodName} threw an exception. Exception Message: {Message}. Inner Exception: {InnerException}", 
                methodName, ex.Message, ex.InnerException?.Message ?? "None");
        }

        public static void SendError<T>(this ILogger<T> logger, string message, Exception ex, [CallerMemberName] string methodName = "")
        {
            logger.LogError(ex, "{MethodName}: {Message}. Exception Message: {ExceptionMessage}. Inner Exception: {InnerException}", 
                methodName, message, ex.Message, ex.InnerException?.Message ?? "None");
        }

        public static void SendError<T>(this ILogger<T> logger, Guid userId, Exception ex, [CallerMemberName] string methodName = "")
        {
            logger.LogError(ex, "{MethodName} | UserId: {UserId} | Exception Message: {Message} | Inner Exception: {InnerException}", 
                methodName, userId, ex.Message, ex.InnerException?.Message ?? "None");
        }
    }
} 