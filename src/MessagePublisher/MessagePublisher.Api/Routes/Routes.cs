namespace MessagePublisher.Api.Routes;

public static class Routes
{
    private const string IdPlaceholder = "{id:guid}";

    private const string Send = $"send";
    public const string SendMessages = $"{Send}/messages";
    public const string SendMessage = $"{Send}/message";
}