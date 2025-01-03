namespace Contracts.Email.Requests;

public class EmailMessageRequest
{
    public required Guid PublisherId { get; set; }
    public required EmailRecipientRequest SendRecipient { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public ICollection<EmailAttachmentRequest> Attachments { get; set; } = [];
}