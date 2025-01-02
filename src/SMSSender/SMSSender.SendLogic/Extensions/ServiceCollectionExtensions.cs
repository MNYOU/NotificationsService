using SMSSender.SendLogic.Interfaces.Managers;
using SMSSender.SendLogic.Interfaces.Services;
using SMSSender.SendLogic.Managers;
using SMSSender.SendLogic.Services;
using Microsoft.Extensions.DependencyInjection;

namespace SMSSender.SendLogic.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSendLogic(this IServiceCollection services)
    {
        services.AddScoped<ISendService, SendService>();
        services.AddScoped<ISendManager, SendManager>();
        return services;
    }
}