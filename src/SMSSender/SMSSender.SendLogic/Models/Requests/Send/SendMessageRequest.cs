namespace SMSSender.SendLogic.Models.Requests.Send;

public class SendMessageRequest
{
    public string Sender { get; set; }
    public string Recipients { get; set; }
    public string Message { get; set; }
}