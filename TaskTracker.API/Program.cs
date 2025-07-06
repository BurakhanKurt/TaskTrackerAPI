using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using TaskTracker.API.Middleware;
using TaskTracker.Application.Extensions;
using TaskTracker.Infrastructure;
using TaskTracker.Infrastructure.Persistence.Seed;
using TaskTracker.Core.Configuration;
using TaskTracker.Core.Exceptions;
using System.Reflection;
using TaskTracker.Infrastructure.Persistence.Context;
using TaskTracker.Application.Helpers;
using TaskTracker.Infrastructure.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

try
{
    Log.Information("Starting TaskTracker API");

    // Bind configuration
    var appSettings = new AppSettings();
    builder.Configuration.GetSection("AppSettings").Bind(appSettings);
    builder.Services.AddSingleton(appSettings);

    //Infrastructure
    builder.Services.AddInfrastructure(builder.Configuration);
    
    //Application
    builder.Services.AddApplicationServices(builder.Configuration);
    builder.Services.AddFluentValidationAutoValidation();

    //CORS
    builder.Services.AddCorsPolicy(appSettings.CorsSettings);

    //Rate Limit
    builder.Services.AddRateLimiting(appSettings.RateLimitSettings);

    // JWT
    var jwtKey = appSettings.JwtSettings.SecretKey;
    if (string.IsNullOrEmpty(jwtKey))
    {
        jwtKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY") 
            ?? throw new InvalidOperationException("JWT Secret Key is not configured. Please set JWT_SECRET_KEY environment variable.");
    }

    // Configure JWT service
    builder.Services.AddSingleton<IJwtService>(new JwtHelper(jwtKey, appSettings.JwtSettings));

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = !string.IsNullOrEmpty(appSettings.JwtSettings.Issuer),
            ValidateAudience = !string.IsNullOrEmpty(appSettings.JwtSettings.Audience),
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = appSettings.JwtSettings.Issuer,
            ValidAudience = appSettings.JwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

    // Controllers
    builder.Services.AddControllers();

    // Swagger
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "Task Tracker API",
            Version = "v1",
            Description = "Görev takip sistemi için REST API",
        });

        // XML Documentation'ı ekle
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        if (File.Exists(xmlPath))
        {
            c.IncludeXmlComments(xmlPath);
        }

        // JWT Authentication için Swagger konfigürasyonu
        c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            Description = "Example: \"Authorization: Bearer {token}\"",
            Name = "Authorization",
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
        {
            {
                new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Reference = new Microsoft.OpenApi.Models.OpenApiReference
                    {
                        Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
            }
        });
    });

    var app = builder.Build();

    // Exception Middleware
    app.UseMiddleware<ExceptionMiddleware>();
    
    var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    app.UseSwagger();
    app.UseSwaggerUI();

    // CORS middleware
    app.UseCors("TaskTrackerCorsPolicy");

    // Rate Limit middleware
    app.UseRateLimiter();

    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    // Seed Data
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<TaskDbContext>();
        await DataSeeder.SeedAsync(context);
    }

    Log.Information("TaskTracker API started successfully");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "TaskTracker API failed to start");
}
finally
{
    Log.CloseAndFlush();
}
