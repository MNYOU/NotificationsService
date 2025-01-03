namespace Contracts.Sms.Requests;

public class SmsMessageRequest
{
    public required Guid PublisherId { get; set; }
    public required SmsRecipientRequest SendRecipient { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public ICollection<SmsAttachmentRequest> Attachments { get; set; } = [];
}