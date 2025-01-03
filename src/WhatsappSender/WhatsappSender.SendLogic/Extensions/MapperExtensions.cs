using System.Collections.Frozen;
using Contracts.WhatsApp.Requests;
using WhatsappSender.SendLogic.Models.DTO.SendModels;

namespace WhatsappSender.SendLogic.Extensions;

internal static class MapperExtensions
{
    public static IReadOnlyCollection<SendMessage> ToApplicationMessages(this IEnumerable<WhatsAppMessageRequest> requests)
    {
        return requests.Select(ToApplicationMessage).ToFrozenSet();
    }

    public static SendMessage ToApplicationMessage(this WhatsAppMessageRequest request)
    {
        var attachments = request.Attachments.ToApplicationAttachments().ToList();
        return new SendMessage()
        {
            Attachments = attachments,
            Title = request.Title,
            Content = request.Content,
            Recipient = request.SendRecipient.PhoneNumber
        };
    }

    private static IReadOnlyCollection<SendAttachment> ToApplicationAttachments(
        this IEnumerable<WhatsAppAttachmentRequest> requests)
    {
        return requests.Select(ToApplicationAttachment).ToFrozenSet();
    }

    private static SendAttachment ToApplicationAttachment(this WhatsAppAttachmentRequest request)
    {
        return new SendAttachment()
        {
            FileName = request.FileName,
            FileUrl = request.PublicUrl,
        };
    }
}