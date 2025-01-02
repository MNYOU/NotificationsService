using CoreLib.Common;
using EmailSender.SendLogic.Models.DTO.SendModels;

namespace EmailSender.SendLogic.Interfaces.Services;

public interface ISendService
{
    public Task<OperationResult<SendMessage>> Send(SendMessage emailMessage);
    public Task<BatchOperationResult<SendMessage>> SendBulk(IEnumerable<SendMessage> messages);
}