using Contracts.Queues;
using Contracts.WhatsApp.Requests;
using CoreLib.Common;
using MessageBroker.Clients.Publishers;
using MessageBroker.Helpers;
using MessageBroker.Settings;
using MessagePublisher.Logic.Interfaces.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MessagePublisher.Logic.Services;

public class WhatsAppPublisherService(
    IOptions<PublisherOptions> consumerOptions,
    IOptions<RabbitMqOption> options,
    ILogger<EmailPublisherService> logger,
    Serializer serializer,
    IServiceProvider serviceProvider)
    : SimplePublishClient(consumerOptions, options, logger, serializer, serviceProvider),
        IPublisherService<WhatsAppMessageRequest>
{
    public async Task<OperationResult> Publish(WhatsAppMessageRequest body)
    {
        var queueName = QueueConstants.WhatsappQueueName;
        await Initialize(queueName);
        await Publish(body, queueName, string.Empty);
        return OperationResult.Success();
    }
}