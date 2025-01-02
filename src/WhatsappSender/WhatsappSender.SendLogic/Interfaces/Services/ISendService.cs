using WhatsappSender.Core.Common;
using WhatsappSender.SendLogic.Models.DTO.SendModels;

namespace WhatsappSender.SendLogic.Interfaces.Services;

public interface ISendService
{
    public Task<OperationResult<SendMessage>> Send(SendMessage emailMessage);
    public Task<BatchOperationResult<SendMessage>> SendBulk(IReadOnlyCollection<SendMessage> messages);
}