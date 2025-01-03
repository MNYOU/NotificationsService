using Microsoft.Extensions.Hosting;
using Serilog;

namespace CoreLib.Logging.Extensions;

public static class ServiceCollectionExtensions
{
    public static IHostBuilder UseCustomizedSerilogLogging(this IHostBuilder hostBuilder)
    {
        return hostBuilder.UseSerilog((context, services, configuration) =>
        {
            configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day);
        });
    }
}