using MessagePublisher.Logic.Interfaces.Managers;
using MessagePublisher.Logic.Interfaces.Services;
using MessagePublisher.Logic.Managers;
using MessagePublisher.Logic.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MessagePublisher.Logic.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPublisherServices(this IServiceCollection services)
    {
        services.AddScoped<IPublisherService, WhatsAppPublisherService>();
        services.AddScoped<IPublisherService, EmailPublisherService>();
        services.AddScoped<IPublisherService, SmsPublisherService>();

        services.AddScoped<ISendManager, SendManager>();

        return services;
    }
}