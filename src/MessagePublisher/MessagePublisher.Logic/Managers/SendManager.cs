using MessagePublisher.Core.Common;
using MessagePublisher.Logic.Extensions;
using MessagePublisher.Logic.Interfaces.Managers;
using MessagePublisher.Logic.Interfaces.Services;
using MessagePublisher.Logic.Models.Requests.Send;

namespace MessagePublisher.Logic.Managers;

public class SendManager(IPublisherService publisherService) : ISendManager
{
    public async Task<OperationResult> Send(SendMessageRequest request)
    {
        var sendModel = request.ToApplicationMessage();
        return await publisherService.Publish(sendModel);
    }
}