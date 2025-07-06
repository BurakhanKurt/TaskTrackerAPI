using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TaskTracker.Application.Common.Behaviors;

namespace TaskTracker.Application.Extensions;

public static class FluentValidationExtension
{
    public static IServiceCollection RegisterFluentValidationCommandValidators(this IServiceCollection services)
    {
        services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

        services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TrimNameBehavior<,>));

        services.AddHttpClient();

        return services;
    }
} 