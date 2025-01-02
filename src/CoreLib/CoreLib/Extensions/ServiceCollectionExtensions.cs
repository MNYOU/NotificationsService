using Microsoft.Extensions.DependencyInjection;

namespace CoreLib.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddImplementationsOfType<TType>(this IServiceCollection services)
    {
        var baseType = typeof(TType);
        var implementationTypes = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => baseType.IsAssignableFrom(type) && !type.IsAbstract)
            .ToList();

        foreach (var implementationType in implementationTypes)
            services.AddTransient(baseType, implementationType);

        return services;
    }
}