using Contracts.Email.Requests;
using Contracts.Sms.Requests;
using Contracts.WhatsApp.Requests;
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
        services.AddScoped<IPublisherService<WhatsAppMessageRequest>, WhatsAppPublisherService>();
        services.AddScoped<IPublisherService<EmailMessageRequest>, EmailPublisherService>();
        services.AddScoped<IPublisherService<SmsMessageRequest>, SmsPublisherService>();
        services.AddScoped<ISendManager, SendManager>();

        return services;
    }
}