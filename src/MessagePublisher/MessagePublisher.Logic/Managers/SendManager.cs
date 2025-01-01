using MessagePublisher.Core.Common;
using MessagePublisher.Logic.Extensions;
using MessagePublisher.Logic.Interfaces.Managers;
using MessagePublisher.Logic.Interfaces.Services;
using MessagePublisher.Logic.Models.Requests.Send;

namespace MessagePublisher.Logic.Managers;

public class SendManager(IEnumerable<IPublisherService> publisherServices) : ISendManager
{
    public async Task<OperationResult> Send(SendMessageRequest request)
    {
        var results = new List<OperationResult>();

        foreach (var publisher in publisherServices)
        {
            var result = await publisher.Publish(request);
            results.Add(result);
        }

        if (!results.Any(x => x.IsFail))
        {
            return OperationResult.Success();
        }

        var errorMessages = results.Where(x => x.IsFail).Select(x => x.Error!.Message).ToList();
        var combinedMessage = string.Join(", ", errorMessages);
        return Error.Internal($"Ошибка при отправке сообщений. {combinedMessage}");
    }
}