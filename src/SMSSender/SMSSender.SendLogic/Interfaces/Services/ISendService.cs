using CoreLib.Common;
using SMSSender.SendLogic.Models.DTO.SendModels;

namespace SMSSender.SendLogic.Interfaces.Services;

public interface ISendService
{
    public Task<OperationResult<SendMessage>> Send(SendMessage smsMessage);
    public Task<BatchOperationResult<SendMessage>> SendBulk(IEnumerable<SendMessage> messages);
}