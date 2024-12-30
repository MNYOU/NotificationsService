using SMSSender.Core.Common;
using SMSSender.SendLogic.Extensions;
using SMSSender.SendLogic.Interfaces.Managers;
using SMSSender.SendLogic.Interfaces.Services;
using SMSSender.SendLogic.Models.DTO.SendModels;
using SMSSender.SendLogic.Models.Requests.Send;

namespace SMSSender.SendLogic.Managers;

public class SendManager(ISendService sendService) : ISendManager
{
    public async Task<OperationResult<SendMessage>> SendMessage(SendMessageRequest smsMessage)
    {
        var sendMessage = smsMessage.ToApplicationMessage();
        var sendResult = await sendService.Send(sendMessage);
        return sendResult;
    }
    public async Task<BatchOperationResult<SendMessage>> SendBulk(IEnumerable<SendMessageRequest> smsMessages)
    {
        var sendMessages = smsMessages.ToApplicationMessages();
        var sendResult = await sendService.SendBulk(sendMessages);
        return sendResult;
    }
}