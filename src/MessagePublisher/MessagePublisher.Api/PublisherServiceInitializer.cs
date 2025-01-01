using MessagePublisher.Logic.Interfaces.Services;

namespace MessagePublisher.Api;

public class PublisherServiceInitializer(IEnumerable<IPublisherService> publisherService) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        foreach (var service in publisherService)
        {
            await service.StartAsync();
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        foreach (var service in publisherService)
        {
            await service.StopAsync();
        }
    }
}