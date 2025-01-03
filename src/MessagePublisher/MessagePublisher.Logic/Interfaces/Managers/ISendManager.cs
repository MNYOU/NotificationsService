using Contracts.Publisher.Requests;
using CoreLib.Common;

namespace MessagePublisher.Logic.Interfaces.Managers;

public interface ISendManager
{
    public Task<OperationResult> Send(SendMessageRequest request);
}