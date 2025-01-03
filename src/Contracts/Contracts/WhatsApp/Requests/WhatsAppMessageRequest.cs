namespace Contracts.WhatsApp.Requests;

public class WhatsAppMessageRequest
{
    public required Guid PublisherId { get; set; }
    public required WhatsAppRecipientRequest SendRecipient { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public ICollection<WhatsAppAttachmentRequest> Attachments { get; set; } = [];
}