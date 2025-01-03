using Contracts.Sms.Requests;
using CoreLib.Common;
using CoreLib.Logging.Extensions;
using Microsoft.Extensions.Logging;
using SMSSender.SendLogic.Extensions;
using SMSSender.SendLogic.Interfaces.Managers;
using SMSSender.SendLogic.Interfaces.Services;
using SMSSender.SendLogic.Models.DTO.SendModels;

namespace SMSSender.SendLogic.Managers;

public class SendManager(ISendService sendService, ILogger<SendManager> logger) : ISendManager
{
    public async Task<OperationResult<SendMessage>> SendMessage(SmsMessageRequest message)
    {
        var sendMessage = message.ToApplicationMessage();
        var sendResult = await sendService.Send(sendMessage);
        logger.LogResult(sendResult);
        return sendResult;
    }

    public async Task<BatchOperationResult<SendMessage>> SendBulk(IEnumerable<SmsMessageRequest> smsMessages)
    {
        var sendMessages = smsMessages.ToApplicationMessages();
        logger.LogDebug("Mapped SendMessageRequest to SendMessage. Content count: {Count}", sendMessages.Count);
        var sendResult = await sendService.SendBulk(sendMessages);
        logger.LogBatchResult(sendResult);
        return sendResult;
    }
}