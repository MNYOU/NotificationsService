using CoreLib.Common;
using MessagePublisher.Logic.Models.Requests.Send;

namespace MessagePublisher.Logic.Interfaces.Managers;

public interface ISendManager
{
    public Task<OperationResult> Send(SendMessageRequest request);
}