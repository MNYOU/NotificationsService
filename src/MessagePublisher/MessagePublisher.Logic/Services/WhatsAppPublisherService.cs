using MessagePublisher.Logic.Models.Options;
using MessagePublisher.Logic.Services.Base;
using Microsoft.Extensions.Options;

namespace MessagePublisher.Logic.Services;

public class WhatsAppPublisherService(IOptions<RabbitMqOption> options)
    : BasePublisherService(options)
{
    internal override string QueueName { get; } = "whatsapp-messages";
}