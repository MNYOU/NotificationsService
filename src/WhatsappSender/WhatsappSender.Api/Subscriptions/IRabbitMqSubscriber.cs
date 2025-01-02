namespace WhatsappSender.Api.Subscriptions;

public interface IRabbitMqSubscriber
{
    public Task Subscribe();
}