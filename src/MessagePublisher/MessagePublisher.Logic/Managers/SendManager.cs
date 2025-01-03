using Contracts.Email.Requests;
using Contracts.Publisher.Requests;
using Contracts.Sms.Requests;
using Contracts.WhatsApp.Requests;
using CoreLib.Common;
using MessagePublisher.Logic.Interfaces.Managers;
using MessagePublisher.Logic.Interfaces.Services;
using MessagePublisher.Logic.Mappers;

namespace MessagePublisher.Logic.Managers;

public class SendManager(
    IEnumerable<IPublisherManager> publisherManagers) 
    : ISendManager
{
    public async Task<OperationResult> Send(SendMessageRequest request)
    {
        var results = new List<OperationResult>();
        var sendMessages = request.ToSendMessage();

        foreach (var publisherManager in publisherManagers)
        {
            results.Add(await publisherManager.Send(sendMessages));
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