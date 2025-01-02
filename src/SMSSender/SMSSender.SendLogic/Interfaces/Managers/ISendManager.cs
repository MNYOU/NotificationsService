using SMSSender.Core.Common;
using SMSSender.SendLogic.Models.DTO.SendModels;
using SMSSender.SendLogic.Models.Requests.Send;

namespace SMSSender.SendLogic.Interfaces.Managers;

public interface ISendManager
{
    public Task<OperationResult<SendMessage>> SendMessage(SendMessageRequest smsMessage);
    public Task<BatchOperationResult<SendMessage>> SendBulk(IEnumerable<SendMessageRequest> messages);
}