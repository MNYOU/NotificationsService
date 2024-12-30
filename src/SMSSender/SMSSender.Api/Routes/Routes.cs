namespace SMSSender.Api.Routes;

public static class Routes
{
    private const string Send = $"sms/send";
    public const string SendMessages = $"{Send}/messages";
    public const string SendMessage = $"{Send}/message";
}