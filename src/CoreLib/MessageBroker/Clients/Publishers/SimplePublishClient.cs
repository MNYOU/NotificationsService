using MessageBroker.Clients.Base;
using MessageBroker.Helpers;
using MessageBroker.Settings;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace MessageBroker.Clients.Publishers;

public class SimplePublishClient : RabbitClientBase, IPublishClient
{
    private readonly PublisherOptions consumerOptions;

    protected SimplePublishClient(PublisherOptions consumerOptions, ILogger<SimplePublishClient> logger, Serializer serializer, IServiceProvider serviceProvider)
        : base(consumerOptions.RabbitMqOption, logger, serializer, serviceProvider)
    {
        this.consumerOptions = consumerOptions;
    }

    public async Task Initialize(string exchange, string type = ExchangeType.Fanout)
    {
        await Channel.ExchangeDeclareAsync(exchange, type);
    }

    public async Task Publish<TBody>(TBody body, string exchange, string routingKey) where TBody : class
    {
        var bytes = Serializer.GetBytes(body);
        Logger.LogInformation("Start publish message");
        await Channel.BasicPublishAsync(exchange, routingKey, bytes);
        Logger.LogInformation("End publish message");
    }
}