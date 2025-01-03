using Contracts.Email.Requests;
using EmailSender.Api.Consumers.Interfaces;
using EmailSender.SendLogic.Interfaces.Managers;
using MessageBroker.Clients.Consumers;
using MessageBroker.Helpers;
using MessageBroker.Settings;
using Microsoft.Extensions.Options;

namespace EmailSender.Api.Consumers;

public class RabbitMqConsumer(
    IOptions<ConsumerOptions> consumerOptions,
    ILogger<ConsumeClientBase<EmailMessageRequest>> logger,
    Serializer serializer,
    IServiceProvider serviceProvider)
    : ConsumeClientBase<EmailMessageRequest>(consumerOptions, logger, serializer, serviceProvider), IConsumerService
{
    protected override async Task<bool> ProcessMessage(EmailMessageRequest body, IServiceScope scope)
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