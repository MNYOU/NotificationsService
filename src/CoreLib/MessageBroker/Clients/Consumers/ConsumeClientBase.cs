using JetBrains.Annotations;
using MessageBroker.Helpers;
using MessageBroker.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MessageBroker.Clients.Consumers;

[PublicAPI]
public abstract class ConsumeClientBase<T> : BackgroundService, IAsyncDisposable where T : class
{
    private readonly IConnectionFactory connectionFactory;
    private readonly ConsumerOptions consumerOptions;
    private readonly ILogger<ConsumeClientBase<T>> logger;
    private readonly Serializer serializer;
    private readonly IServiceProvider serviceProvider;

    private IChannel? channel;
    private IConnection? connection;

    protected ConsumeClientBase(IOptions<ConsumerOptions> consumerOptions, ILogger<ConsumeClientBase<T>> logger,
        Serializer serializer, IServiceProvider serviceProvider)
    {
        this.logger = logger;
        this.serializer = serializer;
        this.serviceProvider = serviceProvider;

        this.consumerOptions = consumerOptions.Value;
        var rabbitOptions = this.consumerOptions.RabbitMqOption;
        connectionFactory = new ConnectionFactory
        {
            HostName = rabbitOptions.HostName,
            UserName = rabbitOptions.UserName,
            Password = rabbitOptions.Password
        };
    }

    private IConnection Connection
    {
        get
        {
            if (connection?.IsOpen != true)
                connection = connectionFactory.CreateConnectionAsync().GetAwaiter().GetResult();

            return connection;
        }
    }

    private IChannel Channel
    {
        get
        {
            if (channel?.IsOpen != true)
                channel = Connection.CreateChannelAsync().GetAwaiter().GetResult();

            return channel;
        }
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return StartConsuming(consumerOptions.QueueName, stoppingToken);
    }

    private async Task StartConsuming(string queueName, CancellationToken cancellationToken)
    {
        await Channel.QueueDeclareAsync(queueName, false, false, false, cancellationToken: cancellationToken);

        var consumer = new AsyncEventingBasicConsumer(Channel);

        consumer.ReceivedAsync += async (_, ea) =>
        {
            var bytes = ea.Body.ToArray();

            var processedSuccessfully = false;
            try
            {
                processedSuccessfully = await ProcessMessageInternal(bytes);
            }
            catch (Exception ex)
            {
                logger.LogError($"Exception occurred while processing message from queue {queueName}: {ex}");
            }

            if (processedSuccessfully)
                await Channel.BasicAckAsync(ea.DeliveryTag, false, cancellationToken);
            else
                await Channel.BasicRejectAsync(ea.DeliveryTag, true, cancellationToken);
        };

        await Channel.BasicConsumeAsync(queueName, false, consumer, cancellationToken);
    }

    private Task<bool> ProcessMessageInternal(byte[] bytes)
    {
        var body = serializer.Deserialize<T>(bytes);
        if (body is null)
        {
            return Task.FromResult(false);
        }

        using var scope = serviceProvider.CreateScope();

        return ProcessMessage(body, scope);
    }

    protected abstract Task<bool> ProcessMessage(T body, IServiceScope scope);

    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore();
        GC.SuppressFinalize(this);
    }

    protected virtual async ValueTask DisposeAsyncCore()
    {
        if (channel != null)
            await channel.DisposeAsync();
        if (connection != null)
            await connection.DisposeAsync();
    }
}