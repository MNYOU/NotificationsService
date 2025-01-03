namespace Contracts.Publisher.Requests;

public class SendMessageRequest
{
    public required SendRecipientRequest SendRecipient { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public ICollection<SendAttachmentRequest> Attachments { get; set; } = [];
}