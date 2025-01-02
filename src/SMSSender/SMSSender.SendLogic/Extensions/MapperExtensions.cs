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
        return new SendMessage()
        {
            Sender = request.Sender,
            Recipient = request.Recipient,
            Message = request.Message,
        };
    }
}