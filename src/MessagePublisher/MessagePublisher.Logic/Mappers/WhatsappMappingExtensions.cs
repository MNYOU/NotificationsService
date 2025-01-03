using Contracts.WhatsApp.Requests;
using MessagePublisher.Logic.Models.DTO.SendModels;

namespace MessagePublisher.Logic.Mappers;

public static class WhatsappMappingExtensions
{
    public static WhatsAppMessageRequest ToWhatsAppMessageRequest(this SendMessage message)
    {
        ArgumentNullException.ThrowIfNull(message);
        return new WhatsAppMessageRequest
        {
            PublisherId = message.Id,
            SendRecipient = message.Recipient.ToWhatsAppRecipientRequest(),
            Title = message.Title,
            Content = message.Content,
            Attachments = message.Attachments?.Select(a => a.ToWhatsAppAttachmentRequest()).ToList() ?? []
        };
    }

    private static WhatsAppRecipientRequest ToWhatsAppRecipientRequest(this Recipient recipient)
    {
        ArgumentNullException.ThrowIfNull(recipient);
        return new WhatsAppRecipientRequest
        {
            PhoneNumber = recipient.WhatsappNumber
        };
    }
    
    private static WhatsAppAttachmentRequest ToWhatsAppAttachmentRequest(this SendAttachment attachment)
    {
        ArgumentNullException.ThrowIfNull(attachment);
        return new WhatsAppAttachmentRequest
        {
            FileName = attachment.FileName,
            PublicUrl = attachment.FileUrl
        };
    }
}