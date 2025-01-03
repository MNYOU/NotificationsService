using CoreLib.Common;
using CoreLib.Logging.Extensions;
using EmailSender.SendLogic.Extensions;
using EmailSender.SendLogic.Interfaces.Managers;
using EmailSender.SendLogic.Interfaces.Services;
using EmailSender.SendLogic.Models.DTO.SendModels;
using EmailSender.SendLogic.Models.Requests.Send;
using Microsoft.Extensions.Logging;

namespace EmailSender.SendLogic.Managers;

public class SendManager(ISendService sendService, ILogger<SendManager> logger) : ISendManager
{
    public async Task<BatchOperationResult<SendMessage>> SendBulk(ICollection<SendMessageRequest> messages)
    {
        logger.LogInformation("Received bulk send messages request. Message count: {Count}", messages.Count);
        var sendMessages = messages.ToApplicationMessages();
        logger.LogDebug("Mapped SendMessageRequest to SendMessage. Message count: {Count}", sendMessages.Count);
        var sendResult = await sendService.SendBulk(sendMessages);
        logger.LogBatchResult(sendResult);
        return sendResult;
    }
    
    public async Task<OperationResult<SendMessage>> SendMessage(SendMessageRequest message)
    {
        logger.LogInformation("Received send message request for recipient: {Recipient}", message.Recipient);
        var sendMessage = message.ToApplicationMessage();
        logger.LogDebug("Mapped SendMessageRequest to SendMessage for recipient: {Recipient}", message.Recipient);
        var sendResult = await sendService.Send(sendMessage);
        logger.LogResult(sendResult);
        return sendResult;
    }
}