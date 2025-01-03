using Contracts.Sms.Requests;
using MessageBroker.Clients.Consumers;
using MessageBroker.Helpers;
using MessageBroker.Settings;
using Microsoft.Extensions.Options;
using SMSSender.Api.Consumers.Interfaces;
using SMSSender.SendLogic.Interfaces.Managers;

namespace SMSSender.Api.Consumers;

public class RabbitMqConsumer(
    IOptions<ConsumerOptions> consumerOptions,
    ILogger<ConsumeClientBase<SmsMessageRequest>> logger,
    Serializer serializer,
    IServiceProvider serviceProvider)
    : ConsumeClientBase<SmsMessageRequest>(consumerOptions, logger, serializer, serviceProvider), IConsumerService
{
    protected override async Task<bool> ProcessMessage(SmsMessageRequest body, IServiceScope scope)
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