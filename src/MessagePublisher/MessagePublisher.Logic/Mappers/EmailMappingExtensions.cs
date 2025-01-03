using Contracts.Email.Requests;
using MessagePublisher.Logic.Models.DTO.SendModels;

namespace MessagePublisher.Logic.Mappers;

public static class EmailMappingExtensions
{
    public static EmailMessageRequest ToEmailMessageRequest(this SendMessage message)
    {
        ArgumentNullException.ThrowIfNull(message);
        return new EmailMessageRequest
        {
            PublisherId = message.Id,
            SendRecipient = message.Recipient.ToEmailRecipientRequest(),
            Title = message.Title,
            Content = message.Content,
            Attachments = message.Attachments?.Select(a => a.ToEmailAttachmentRequest()).ToList() ?? []
        };
    }

    private static EmailRecipientRequest ToEmailRecipientRequest(this Recipient recipient)
    {
        ArgumentNullException.ThrowIfNull(recipient);
        return new EmailRecipientRequest
        { 
            Email = recipient.Email
        };
    }
    
    private static EmailAttachmentRequest ToEmailAttachmentRequest(this SendAttachment attachment)
    {
        ArgumentNullException.ThrowIfNull(attachment);
        return new EmailAttachmentRequest
        {
            FileName = attachment.FileName,
            PublicUrl = attachment.FileUrl
        };
    }
}