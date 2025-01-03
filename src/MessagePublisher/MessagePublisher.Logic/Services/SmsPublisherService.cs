using Contracts.Queues;
using Contracts.Sms.Requests;
using CoreLib.Common;
using MessageBroker.Clients.Publishers;
using MessageBroker.Helpers;
using MessageBroker.Settings;
using MessagePublisher.Logic.Interfaces.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MessagePublisher.Logic.Services;

public class SmsPublisherService(
    IOptions<PublisherOptions> consumerOptions,
    IOptions<RabbitMqOption> options,
    ILogger<EmailPublisherService> logger,
    Serializer serializer,
    IServiceProvider serviceProvider)
    : SimplePublishClient(consumerOptions, options, logger, serializer, serviceProvider),
        IPublisherService<SmsMessageRequest>
{
    public async Task<OperationResult> Publish(SmsMessageRequest body)
    {
        var queueName = QueueConstants.SmsQueueName;
        await Initialize(queueName);
        await Publish(body, queueName, string.Empty);
        return OperationResult.Success();
    }
}