using WhatsappSender.Api.Consumers.Interfaces;

namespace WhatsappSender.Api.Consumers;

public class ConsumerServiceInitializer(IConsumerService service) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await service.StartAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await service.StopAsync();
    }
}