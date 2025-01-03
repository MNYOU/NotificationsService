
namespace MessageBroker.Settings;

public class ConsumerOptions
{
    public string QueueName { get; set; } = string.Empty;
    public RabbitMqOption RabbitMqOption { get; set; } = new();
}