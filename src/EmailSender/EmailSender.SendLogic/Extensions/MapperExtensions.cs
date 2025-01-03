using System.Collections.Frozen;
using Contracts.Email.Requests;
using EmailSender.SendLogic.Models.DTO.SendModels;

namespace EmailSender.SendLogic.Extensions;

internal static class MapperExtensions
{
    public static IReadOnlyCollection<SendMessage> ToApplicationMessages(this IEnumerable<EmailMessageRequest> requests)
    {
        return requests.Select(ToApplicationMessage).ToFrozenSet();
    }
    
    public static SendMessage ToApplicationMessage(this EmailMessageRequest request)
    {
        var attachments = request.Attachments.ToApplicationAttachments().ToList();
        return new SendMessage()
        {
            Attachments = attachments,
            Title = request.Title,
            Content = request.Content,
            Recipient = request.SendRecipient.Email
        };
    }

    private static IReadOnlyCollection<SendAttachment> ToApplicationAttachments(this IEnumerable<EmailAttachmentRequest> requests)
    {
        return requests.Select(ToApplicationAttachment).ToFrozenSet();
    }

    private static SendAttachment ToApplicationAttachment(this EmailAttachmentRequest request)
    {
        return new SendAttachment()
        {
            FileName = request.FileName,
            FileUrl =  request.PublicUrl,
        };
    }
}