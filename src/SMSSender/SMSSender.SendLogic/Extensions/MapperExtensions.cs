using System.Collections.Frozen;
using Contracts.Sms.Requests;
using SMSSender.SendLogic.Models.DTO.SendModels;

namespace SMSSender.SendLogic.Extensions;

internal static class MapperExtensions
{
    public static IReadOnlyCollection<SendMessage> ToApplicationMessages(this IEnumerable<SmsMessageRequest> requests)
    {
        return requests.Select(ToApplicationMessage).ToFrozenSet();
    }

    public static SendMessage ToApplicationMessage(this SmsMessageRequest request)
    {
        var attachments = request.Attachments.ToApplicationAttachments().ToList();
        return new SendMessage()
        {
            Recipient = request.SendRecipient.PhoneNumber,
            Content = request.Content,
            Attachments = attachments
        };
    }

    private static IReadOnlyCollection<SendAttachment> ToApplicationAttachments(
        this IEnumerable<SmsAttachmentRequest> requests)
    {
        return requests.Select(ToApplicationAttachment).ToFrozenSet();
    }

    private static SendAttachment ToApplicationAttachment(this SmsAttachmentRequest request)
    {
        return new SendAttachment()
        {
            FileName = request.FileName,
            FileUrl = request.PublicUrl,
        };
    }
}