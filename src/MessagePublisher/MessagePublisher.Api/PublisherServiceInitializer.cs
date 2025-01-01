using MessagePublisher.Logic.Interfaces.Services;

namespace MessagePublisher.Api;

public class PublisherServiceInitializer(IPublisherService publisherService) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await publisherService.StartAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await publisherService.StopAsync();
    }
}