using MessagePublisher.Logic.Models.Options;
using MessagePublisher.Logic.Services.Base;
using Microsoft.Extensions.Options;

namespace MessagePublisher.Logic.Services;

public class SmsPublisherService(IOptions<RabbitMqOption> options)
    : BasePublisherService(options)
{
    internal override string QueueName { get; } = "sms-messages";
}