using MediatR;
using System.Reflection;

namespace TaskTracker.Application.Common.Behaviors;

public class TrimNameBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        TrimStringProperties(request);
        return await next();
    }

    private static void TrimStringProperties(object obj)
    {
        if (obj == null) return;

        var type = obj.GetType();
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.PropertyType == typeof(string) && p.CanWrite);

        foreach (var property in properties)
        {
            var value = property.GetValue(obj) as string;
            if (value != null)
            {
                property.SetValue(obj, value.Trim());
            }
        }
    }
} 