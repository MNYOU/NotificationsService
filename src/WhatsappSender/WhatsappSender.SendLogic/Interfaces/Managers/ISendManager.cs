using Contracts.WhatsApp.Requests;
using CoreLib.Common;
using WhatsappSender.SendLogic.Models.DTO.SendModels;

namespace WhatsappSender.SendLogic.Interfaces.Managers;

public interface ISendManager
{
    public Task<BatchOperationResult<SendMessage>> SendBulk(ICollection<WhatsAppMessageRequest> messages);
    public Task<OperationResult<SendMessage>> SendMessage(WhatsAppMessageRequest message);
}