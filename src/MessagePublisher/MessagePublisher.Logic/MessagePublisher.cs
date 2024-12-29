using System.Text;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace MessagePublisher.Logic;

public class RabbitMqPublisherService(IOptions<RabbitMqOption> options) : IRabbitMqPublisherService
{
    private readonly string hostName = options.Value.HostName;

    public async Task Publish(string message)
    {
        var factory = new ConnectionFactory { HostName = hostName };
        await using var connection = await factory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();
        await channel.ExchangeDeclareAsync(exchange: "messages", type: ExchangeType.Fanout);
        var body = Encoding.UTF8.GetBytes(message);
        await channel.BasicPublishAsync(exchange: "messages", routingKey: string.Empty, body: body);
    }
}