namespace SMSSender.SendLogic.Models.Requests.Send;

public class SendMessageRequest
{
    public string Sender { get; set; }
    public string Recipient { get; set; }
    public string Content { get; set; }
    public ICollection<SendAttachmentRequest> Attachments { get; set; } = [];
}