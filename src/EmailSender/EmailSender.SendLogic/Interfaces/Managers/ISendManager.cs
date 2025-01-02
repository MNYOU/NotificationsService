using CoreLib.Common;
using EmailSender.SendLogic.Models.DTO.SendModels;
using EmailSender.SendLogic.Models.Requests.Send;

namespace EmailSender.SendLogic.Interfaces.Managers;

public interface ISendManager
{
    public Task<BatchOperationResult<SendMessage>> SendBulk(IEnumerable<SendMessageRequest> messages);
    public Task<OperationResult<SendMessage>> SendMessage(SendMessageRequest message);
}