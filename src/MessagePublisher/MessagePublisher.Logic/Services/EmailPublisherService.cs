using Contracts.Email.Requests;
using Contracts.Queues;
using CoreLib.Common;
using MessageBroker.Clients.Publishers;
using MessageBroker.Helpers;
using MessageBroker.Settings;
using MessagePublisher.Logic.Interfaces.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MessagePublisher.Logic.Services;

public class EmailPublisherService(
    IOptions<PublisherOptions> consumerOptions,
    IOptions<RabbitMqOption> options,
    ILogger<EmailPublisherService> logger,
    Serializer serializer,
    IServiceProvider serviceProvider)
    : SimplePublishClient(consumerOptions, options, logger, serializer, serviceProvider),
        IPublisherService<EmailMessageRequest>
{
    public async Task<OperationResult> Publish(EmailMessageRequest body)
    {
        var queueName = QueueConstants.EmailQueueName;
        await Initialize(queueName);
        await Publish(body, queueName, string.Empty);
        return OperationResult.Success();
    }
}