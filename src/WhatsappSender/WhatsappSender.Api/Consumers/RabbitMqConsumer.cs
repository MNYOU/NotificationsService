using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using WhatsappSender.Api.Consumers.Interfaces;
using WhatsappSender.Api.Consumers.Options;
using WhatsappSender.Core.Common;
using WhatsappSender.SendLogic.Interfaces.Managers;
using WhatsappSender.SendLogic.Models.Requests.Send;

namespace WhatsappSender.Api.Consumers;

public class RabbitMqConsumer : IConsumerService, IAsyncDisposable
{
    private const string queueName = "whatsapp-messages";
    private readonly string hostName;
    private readonly IConnectionFactory connectionFactory;
    private IConnection? connection;
    private IChannel? channel;
    private readonly ISendManager sender;

    public RabbitMqConsumer(IOptions<RabbitMqOption> options, ISendManager sender)
    {
        hostName = options.Value.HostName;
        connectionFactory = new ConnectionFactory { HostName = hostName };
        this.sender = sender;
    }

    public async Task StartAsync()
    {
        connection = await connectionFactory.CreateConnectionAsync();
        channel = await connection.CreateChannelAsync();
        await channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false,
            arguments: null);

        await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var props = ea.BasicProperties;
            var replyProps = new BasicProperties
            {
                CorrelationId = props.CorrelationId
            };

            try
            {
                var parsedMessage = JsonSerializer.Deserialize<SendMessageRequest>(message);
                if (parsedMessage is null)
                {
                    const string errorMessage = "Ошибка десериализации сообщения";
                    var errorResult = Error.Internal(errorMessage);
                    var errorJson = JsonSerializer.Serialize(errorResult);
                    var errorBody = Encoding.UTF8.GetBytes(errorJson);

                    await channel.BasicPublishAsync(exchange: "", routingKey: props.ReplyTo,
                        mandatory: false, basicProperties: replyProps, body: errorBody);
                    await channel.BasicAckAsync(ea.DeliveryTag, false);

                    return;
                }

                var result = await sender.SendMessage(parsedMessage);
                var jsonResult = JsonSerializer.Serialize(result);
                var resultBytes = Encoding.UTF8.GetBytes(jsonResult);

                await channel.BasicPublishAsync(exchange: "", routingKey: props.ReplyTo,
                    mandatory: false, basicProperties: replyProps, body: resultBytes);

                await channel.BasicAckAsync(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Ошибка обработки сообщения: {ex.Message}";
                var errorResult = Error.Internal(errorMessage);
                var errorJson = JsonSerializer.Serialize(errorResult);
                var errorBody = Encoding.UTF8.GetBytes(errorJson);

                await channel.BasicPublishAsync(exchange: "", routingKey: props.ReplyTo,
                    mandatory: false, basicProperties: replyProps, body: errorBody);
                await channel.BasicAckAsync(ea.DeliveryTag, false);
            }
        };

        await channel.BasicConsumeAsync(queue: queueName, autoAck: false, consumer: consumer);
    }

    public async Task StopAsync()
    {
        await DisposeAsync();
    }

    public async ValueTask DisposeAsync()
    {
        if (channel is not null)
        {
            await channel.CloseAsync();
        }

        if (connection is not null)
        {
            await connection.CloseAsync();
        }
    }
}