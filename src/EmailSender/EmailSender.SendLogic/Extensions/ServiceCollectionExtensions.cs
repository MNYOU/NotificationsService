using EmailSender.SendLogic.Interfaces.Managers;
using EmailSender.SendLogic.Interfaces.Services;
using EmailSender.SendLogic.Managers;
using EmailSender.SendLogic.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EmailSender.SendLogic.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSendLogic(this IServiceCollection services)
    {
        services.AddScoped<ISendService, SendService>();
        services.AddScoped<ISendManager, SendManager>();
        return services;
    }
}