using RabbitMQ.Client;

namespace MessageBroker.Clients.Publishers;

public interface IPublishClient
{
    public Task Initialize(string exchange, string type = ExchangeType.Fanout);
    public Task Publish<TBody>(TBody body, string exchange, string routingKey) where TBody : class;
}