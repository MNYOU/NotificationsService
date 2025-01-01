using MessagePublisher.Core.Common;
using MessagePublisher.Logic.Models.Requests.Send;

namespace MessagePublisher.Logic.Interfaces.Services;

public interface IPublisherService
{
    public Task<OperationResult> Publish(SendMessageRequest message);
}