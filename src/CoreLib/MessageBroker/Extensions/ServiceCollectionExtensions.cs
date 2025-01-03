using MessageBroker.Helpers;
using MessageBroker.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MessageBroker.Extensions;

// todo
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSimplePublisher(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMqOption>(configuration.GetSection(nameof(RabbitMqOption)));
        return services;
    }

    public static IServiceCollection AddDirectPublisher(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMqOption>(configuration.GetSection(nameof(RabbitMqOption)));
        return services;
    }

    public static IServiceCollection AddSimpleConsumer(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ConsumerOptions>(configuration.GetSection(nameof(ConsumerOptions)));
        return services;
    }

    public static IServiceCollection AddMessageBrokerServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<Serializer>();
        return services;
    }
}