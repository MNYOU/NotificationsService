using Contracts.Sms.Requests;
using CoreLib.Common;
using SMSSender.SendLogic.Models.DTO.SendModels;

namespace SMSSender.SendLogic.Interfaces.Managers;

public interface ISendManager
{
    public Task<OperationResult<SendMessage>> SendMessage(SmsMessageRequest smsMessage);
    public Task<BatchOperationResult<SendMessage>> SendBulk(IEnumerable<SmsMessageRequest> smsMessages);
}