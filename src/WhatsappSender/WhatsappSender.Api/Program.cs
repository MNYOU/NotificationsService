using Serilog;

namespace WhatsappSender.Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();
        await host.RunAsync();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}