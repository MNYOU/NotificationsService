namespace SMSSender.SendLogic.Models.DTO.SendModels;

public class SendMessage
{
    public string Sender { get; set; } = string.Empty;
    public string Recipient { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}