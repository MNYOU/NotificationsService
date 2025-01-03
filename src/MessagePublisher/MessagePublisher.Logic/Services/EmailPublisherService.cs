using Contracts.Email.Requests;
using Contracts.Queues;
using CoreLib.Common;
using MessageBroker.Clients.Publishers;
using MessageBroker.Helpers;
using MessageBroker.Settings;
using MessagePublisher.Logic.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace MessagePublisher.Logic.Services;

public class EmailPublisherService : SimplePublishClient, IPublisherService<EmailMessageRequest>
{
    protected EmailPublisherService(PublisherOptions consumerOptions, ILogger<EmailPublisherService> logger, 
        Serializer serializer, IServiceProvider serviceProvider) 
        : base(consumerOptions, logger, serializer, serviceProvider)
    {
    }

    public async Task<OperationResult> Publish(EmailMessageRequest body)
    {
        var queueName = QueueConstants.EmailQueueName;
        await Initialize(queueName);
        await Publish(body, queueName, string.Empty);
        return OperationResult.Success();
    }
}