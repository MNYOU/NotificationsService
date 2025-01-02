using CoreLib.Common;
using WhatsappSender.SendLogic.Models.DTO.SendModels;
using WhatsappSender.SendLogic.Models.Requests.Send;

namespace WhatsappSender.SendLogic.Interfaces.Managers;

public interface ISendManager
{
    public Task<BatchOperationResult<SendMessage>> SendBulk(ICollection<SendMessageRequest> messages);
    public Task<OperationResult<SendMessage>> SendMessage(SendMessageRequest message);
}