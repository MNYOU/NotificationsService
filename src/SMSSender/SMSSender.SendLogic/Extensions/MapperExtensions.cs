using System.Collections.Frozen;
using SMSSender.SendLogic.Models.DTO.SendModels;
using SMSSender.SendLogic.Models.Requests.Send;

namespace SMSSender.SendLogic.Extensions;

internal static class MapperExtensions
{
    public static IReadOnlyCollection<SendMessage> ToApplicationMessages(this IEnumerable<SendMessageRequest> requests)
    {
        return requests.Select(ToApplicationMessage).ToFrozenSet();
    }

    public static SendMessage ToApplicationMessage(this SendMessageRequest request)
    {
        var attachments = request.Attachments.ToApplicationAttachments().ToList();
        return new SendMessage()
        {
            Sender = request.Sender,
            Recipient = request.Recipient,
            Content = request.Content,
            Attachments = attachments
        };
    }

    public static IReadOnlyCollection<SendAttachment> ToApplicationAttachments(this IEnumerable<SendAttachmentRequest> requests)
    {
        return requests.Select(ToApplicationAttachment).ToFrozenSet();
    }

    public static SendAttachment ToApplicationAttachment(this SendAttachmentRequest request)
    {
        return new SendAttachment()
        {
            FileName = request.FileName,
            FileUrl = request.PublicUrl,
        };
    }
}