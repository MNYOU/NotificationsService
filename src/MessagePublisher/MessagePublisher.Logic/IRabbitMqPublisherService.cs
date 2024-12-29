namespace MessagePublisher.Logic;

public interface IRabbitMqPublisherService
{
    // TODO: Переделать под модель сообщений
    public Task Publish(string message);
}