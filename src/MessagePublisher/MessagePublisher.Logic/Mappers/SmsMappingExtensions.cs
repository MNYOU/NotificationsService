using Contracts.Sms.Requests;
using MessagePublisher.Logic.Models.DTO.SendModels;

namespace MessagePublisher.Logic.Mappers;

public static class SmsMappingExtensions
{
    public static SmsMessageRequest ToSmsMessageRequest(this SendMessage message)
    {
        ArgumentNullException.ThrowIfNull(message);
        return new SmsMessageRequest
        {
            PublisherId = message.Id,
            SendRecipient = message.Recipient.ToSmsRecipientRequest(),
            Title = message.Title,
            Content = message.Content,
            Attachments = message.Attachments?.Select(a => a.ToSmsAttachmentRequest()).ToList() ?? []
        };
    }

    private static SmsRecipientRequest ToSmsRecipientRequest(this Recipient recipient)
    {
        ArgumentNullException.ThrowIfNull(recipient);
        return new SmsRecipientRequest
        {
            PhoneNumber = recipient.PhoneNumber
        };
    }
    
    private static SmsAttachmentRequest ToSmsAttachmentRequest(this SendAttachment attachment)
    {
        ArgumentNullException.ThrowIfNull(attachment);
        return new SmsAttachmentRequest
        {
            FileName = attachment.FileName,
            PublicUrl = attachment.FileUrl
        };
    }
}