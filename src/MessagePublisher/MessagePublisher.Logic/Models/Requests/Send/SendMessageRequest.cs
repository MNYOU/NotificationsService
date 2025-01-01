namespace MessagePublisher.Logic.Models.Requests.Send;

public class SendMessageRequest
{
    public string Recipient { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public ICollection<SendAttachmentRequest> Attachments { get; set; } = [];
}