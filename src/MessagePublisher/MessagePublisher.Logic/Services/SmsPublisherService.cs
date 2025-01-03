using Contracts.Queues;
using Contracts.Sms.Requests;
using CoreLib.Common;
using MessageBroker.Clients.Publishers;
using MessageBroker.Helpers;
using MessageBroker.Settings;
using MessagePublisher.Logic.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace MessagePublisher.Logic.Services;

public class SmsPublisherService : SimplePublishClient, IPublisherService<SmsMessageRequest>
{
    protected SmsPublisherService(PublisherOptions consumerOptions, ILogger<EmailPublisherService> logger, 
        Serializer serializer, IServiceProvider serviceProvider) 
        : base(consumerOptions, logger, serializer, serviceProvider)
    {
    }
    
    public async Task<OperationResult> Publish(SmsMessageRequest body)
    {
        var queueName = QueueConstants.SmsQueueName;
        await Initialize(queueName);
        await Publish(body, queueName, string.Empty);
        return OperationResult.Success();
    }
}