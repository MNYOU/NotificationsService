using Contracts.WhatsApp.Requests;
using WhatsappSender.Api.Consumers.Interfaces;
using WhatsappSender.SendLogic.Interfaces.Managers;
using MessageBroker.Clients.Consumers;
using MessageBroker.Helpers;
using MessageBroker.Settings;
using Microsoft.Extensions.Options;

namespace WhatsappSender.Api.Consumers;

public class RabbitMqConsumer(
    IOptions<ConsumerOptions> consumerOptions,
    ILogger<ConsumeClientBase<WhatsAppMessageRequest>> logger,
    Serializer serializer,
    IServiceProvider serviceProvider)
    : ConsumeClientBase<WhatsAppMessageRequest>(consumerOptions, logger, serializer, serviceProvider), IConsumerService
{
    protected override async Task<bool> ProcessMessage(WhatsAppMessageRequest body, IServiceScope scope)
    {
        var sendManager = scope.ServiceProvider.GetRequiredService<ISendManager>();
        var result = await sendManager.SendMessage(body);
        return result.IsSuccess;
    }

    public async Task StartAsync()
    {
        await ExecuteAsync(CancellationToken.None);
    }

    public async Task StopAsync()
    {
        await DisposeAsync();
    }
}