namespace SMSSender.Domain.Settings;

public class SMSSettings
{
    public string Project { get; set; } = "SMSSender";
    public string ApiKey { get; set; } = "";
    public string Login { get; set; } = "";
    public string Password { get; set; } = "";
    public string Sender { get; set; } = "SMSC.RU";
}