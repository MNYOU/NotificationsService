using MessageBroker.Helpers;
using MessageBroker.Settings;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace MessageBroker.Clients.Base;

public abstract class RabbitClientBase
{
    private readonly IConnectionFactory connectionFactory;

    private IChannel? channel;
    private IConnection? connection;

    protected RabbitClientBase(RabbitMqOption options, ILogger<RabbitClientBase> logger, Serializer serializer,
        IServiceProvider serviceProvider)
    {
        Logger = logger;
        Serializer = serializer;
        ServiceProvider = serviceProvider;

        RabbitMqOption = options;
        connectionFactory = new ConnectionFactory
        {
            HostName = RabbitMqOption.HostName,
            UserName = RabbitMqOption.UserName,
            Password = RabbitMqOption.Password
        };
    }

    protected RabbitMqOption RabbitMqOption { get; }
    protected ILogger<RabbitClientBase> Logger { get; }
    protected Serializer Serializer { get; }
    protected IServiceProvider ServiceProvider { get; }

    protected IConnection Connection
    {
        get
        {
            if (connection?.IsOpen != true)
                connection = connectionFactory.CreateConnectionAsync().GetAwaiter().GetResult();

            return connection;
        }
    }

    protected IChannel Channel
    {
        get
        {
            if (channel?.IsOpen != true)
                channel = Connection.CreateChannelAsync().GetAwaiter().GetResult();

            return channel;
        }
    }
}