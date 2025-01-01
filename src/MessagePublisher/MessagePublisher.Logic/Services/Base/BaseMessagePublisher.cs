using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;
using MessagePublisher.Core.Common;
using MessagePublisher.Logic.Interfaces.Services;
using MessagePublisher.Logic.Models.DTO.SendModels;
using MessagePublisher.Logic.Models.Options;
using MessagePublisher.Logic.Models.Requests.Send;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MessagePublisher.Logic.Services.Base;

public abstract class BasePublisherService : IPublisherService, IAsyncDisposable
{
    internal virtual string QueueName { get; }
    private readonly string hostName;
    private readonly IConnectionFactory connectionFactory;
    private readonly ConcurrentDictionary<string, TaskCompletionSource<string>> callbackMapper = new();
    private IConnection? connection;
    private IChannel? channel;
    private string? replyQueueName;

    internal BasePublisherService(IOptions<RabbitMqOption> options)
    {
        hostName = options.Value.HostName;
        connectionFactory = new ConnectionFactory { HostName = hostName };
    }

    public async Task StartAsync()
    {
        connection = await connectionFactory.CreateConnectionAsync();
        channel = await connection.CreateChannelAsync();
        var queueDeclareResult = await channel.QueueDeclareAsync();
        replyQueueName = queueDeclareResult.QueueName;
        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += (model, ea) =>
        {
            var correlationId = ea.BasicProperties.CorrelationId;

            if (string.IsNullOrEmpty(correlationId))
            {
                return Task.CompletedTask;
            }

            if (!callbackMapper.TryRemove(correlationId, out var tcs))
            {
                return Task.CompletedTask;
            }

            var body = ea.Body.ToArray();
            var response = Encoding.UTF8.GetString(body);
            tcs.TrySetResult(response);
            return Task.CompletedTask;
        };

        await channel.BasicConsumeAsync(replyQueueName, true, consumer);
    }

    public async Task<OperationResult<SendMessage>> Publish(SendMessageRequest message)
    {
        if (channel is null)
        {
            return Error.Internal("RPC client not started");
        }

        var correlationId = Guid.NewGuid().ToString();
        var props = new BasicProperties
        {
            CorrelationId = correlationId,
            ReplyTo = replyQueueName
        };
        var tcs = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);
        callbackMapper.TryAdd(correlationId, tcs);

        var jsonMessage = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(jsonMessage);

        await channel.BasicPublishAsync(exchange: string.Empty, routingKey: QueueName,
            mandatory: true, basicProperties: props, body: body);
        try
        {
            var response = await tcs.Task;
            if (string.IsNullOrEmpty(response))
            {
                return Error.Internal("Сервер вернул пустой результат");
            }

            var parsedResponse = JsonSerializer.Deserialize<OperationResult<SendMessage>>(response);
            return parsedResponse ?? Error.Internal("После десериализации модель = Null");
        }
        catch (TaskCanceledException)
        {
            return Error.Internal("Запрос отменён.");
        }
        catch (Exception ex)
        {
            return Error.Internal($"Ошибка при отправке сообщения: {ex.Message}");
        }
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