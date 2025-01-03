using Contracts.Queues;
using Contracts.WhatsApp.Requests;
using CoreLib.Common;
using MessageBroker.Clients.Publishers;
using MessageBroker.Helpers;
using MessageBroker.Settings;
using MessagePublisher.Logic.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace MessagePublisher.Logic.Services;

public class WhatsAppPublisherService : SimplePublishClient, IPublisherService<WhatsAppMessageRequest>
{
    protected WhatsAppPublisherService(PublisherOptions consumerOptions, ILogger<EmailPublisherService> logger, 
        Serializer serializer, IServiceProvider serviceProvider) 
        : base(consumerOptions, logger, serializer, serviceProvider)
    {
    }

    public async Task<OperationResult> Publish(WhatsAppMessageRequest body)
    {
        var queueName = QueueConstants.WhatsappQueueName;
        await Initialize(queueName);
        await Publish(body, queueName, string.Empty);
        return OperationResult.Success();
    }
}