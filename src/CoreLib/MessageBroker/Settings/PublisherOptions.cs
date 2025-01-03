namespace MessageBroker.Settings;

public class PublisherOptions
{
    public RabbitMqOption RabbitMqOption { get; set; } = new();
}