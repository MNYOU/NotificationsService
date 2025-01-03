using Contracts.Publisher.Requests;
using MessagePublisher.Logic.Models.DTO.SendModels;

namespace MessagePublisher.Logic.Mappers;

public static class SendMappingExtensions
{
    public static SendMessage ToSendMessage(this SendMessageRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return new SendMessage
        {
            Recipient = request.SendRecipient.ToRecipient(),
            Title = request.Title,
            Content = request.Content,
            Attachments = request.Attachments?.Select(a => a.ToSendAttachment()).ToList() ?? []
        };
    }
    
    private static Recipient ToRecipient(this SendRecipientRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return new Recipient
        {
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            WhatsappNumber = ConvertToWhatsappNumber(request.PhoneNumber)
        };
    }

    private static string ConvertToWhatsappNumber(string phoneNumber)
    {
        ArgumentNullException.ThrowIfNull(phoneNumber);
        return string.Concat("+7", phoneNumber);
    }

    private static SendAttachment ToSendAttachment(this SendAttachmentRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return new SendAttachment
        {
            FileName = request.FileName,
            FileUrl = request.PublicUrl
        };
    }
}