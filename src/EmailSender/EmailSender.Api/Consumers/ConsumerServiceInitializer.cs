using EmailSender.Api.Consumers.Interfaces;

namespace EmailSender.Api.Consumers;

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