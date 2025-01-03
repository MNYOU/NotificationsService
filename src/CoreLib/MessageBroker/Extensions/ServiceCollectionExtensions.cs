using Microsoft.Extensions.DependencyInjection;

namespace MessageBroker.Extensions;

// todo
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSimplePublisher(this IServiceCollection services)
    {
        return services;
    }

    public static IServiceCollection AddDirectPublisher(this IServiceCollection services)
    {
        return services;
    }

    public static IServiceCollection AddSimpleConsumer(this IServiceCollection services)
    {
        return services;
    }
}