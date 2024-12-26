using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using WhatsappBusiness.CloudApi.Configurations;
using WhatsappBusiness.CloudApi.Extensions;
using WhatsappSender.SendLogic.Interfaces.Managers;
using WhatsappSender.SendLogic.Interfaces.Services;
using WhatsappSender.SendLogic.Managers;
using WhatsappSender.SendLogic.Services;

namespace WhatsappSender.SendLogic.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSendLogic(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ISendService, SendService>();
        services.AddScoped<ISendManager, SendManager>();
        services.AddScoped<IModelsService, ModelsService>();
        services.Configure<WhatsAppBusinessCloudApiConfig>(
            configuration.GetSection(nameof(WhatsAppBusinessCloudApiConfig)));
        var serviceProvider = services.BuildServiceProvider();
        var config = serviceProvider.GetRequiredService<IOptions<WhatsAppBusinessCloudApiConfig>>().Value;
        services.AddWhatsAppBusinessCloudApiService(config);
        return services;
    }
}