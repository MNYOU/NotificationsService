using Microsoft.Extensions.DependencyInjection;

namespace MessagePublisher.Logic.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPublisher(this IServiceCollection services)
    {
        services.AddScoped<IRabbitMqPublisherService, RabbitMqPublisherService>();

        return services;
    }
}