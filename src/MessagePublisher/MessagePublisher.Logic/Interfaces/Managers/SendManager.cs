using MessagePublisher.Logic.Models.Requests.Send;
using WhatsappSender.Core.Common;

namespace MessagePublisher.Logic.Interfaces.Managers;

public interface ISendManager
{
    public Task<OperationResult> Send(SendMessageRequest request);
}