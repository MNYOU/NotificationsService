using System.Text;
using System.Text.Json;
using MessagePublisher.Core.Common;
using MessagePublisher.Logic.Interfaces.Services;
using MessagePublisher.Logic.Models.DTO.SendModels;
using MessagePublisher.Logic.Models.Options;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace MessagePublisher.Logic.Services;

public class PublisherService(IOptions<RabbitMqOption> options) : IPublisherService
{
    private readonly string hostName = options.Value.HostName;

    public async Task<OperationResult> Publish(SendMessage message)
    {
        var factory = new ConnectionFactory { HostName = hostName };
        await using var connection = await factory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();
        await channel.ExchangeDeclareAsync(exchange: "messages", type: ExchangeType.Fanout);
        var jsonMessage = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(jsonMessage);
        await channel.BasicPublishAsync(exchange: "messages", routingKey: string.Empty, body: body);
        return OperationResult.Success();
    }
}