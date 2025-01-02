using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace WhatsappSender.Api.Subscriptions;

public class RabbitMqSubscriber : IRabbitMqSubscriber
{
    public async Task Subscribe()
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        await using var connection = await factory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();

        await channel.ExchangeDeclareAsync(exchange: "messages", type: ExchangeType.Fanout);
        var queueDeclareResult = await channel.QueueDeclareAsync();
        var queueName = queueDeclareResult.QueueName;
        await channel.QueueBindAsync(queue: queueName, exchange: "messages", routingKey: string.Empty);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += (_, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            return Task.CompletedTask;
        };

        await channel.BasicConsumeAsync(queueName, autoAck: true, consumer: consumer);
    }
}