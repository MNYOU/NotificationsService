using CoreLib.Common;
using Microsoft.Extensions.Logging;
using SMSSender.SendLogic.Extensions;
using SMSSender.SendLogic.Interfaces.Managers;
using SMSSender.SendLogic.Interfaces.Services;
using SMSSender.SendLogic.Models.DTO.SendModels;
using SMSSender.SendLogic.Models.Requests.Send;

namespace SMSSender.SendLogic.Managers;

public class SendManager(ISendService sendService, ILogger<SendManager> logger) : ISendManager
{
    public async Task<OperationResult<SendMessage>> SendMessage(SendMessageRequest message)
    {
        var sendMessage = message.ToApplicationMessage();
        var sendResult = await sendService.Send(sendMessage);
        
        if(sendResult.IsFail)
        {
            logger.LogError("Failed to send message to recipient: {Recipient}. Error: {Error}", message.Recipient, sendResult.Error); 
        }
        else
        {
            logger.LogInformation("Successfully sent message to recipient: {Recipient}", message.Recipient);
        }
        
        return sendResult;
    }
    public async Task<BatchOperationResult<SendMessage>> SendBulk(IEnumerable<SendMessageRequest> smsMessages)
    {
        var sendMessages = smsMessages.ToApplicationMessages();
        logger.LogDebug("Mapped SendMessageRequest to SendMessage. Content count: {Count}", sendMessages.Count);
        var sendResult = await sendService.SendBulk(sendMessages);

        if(sendResult.IsFailure)
        {
            logger.LogError("Failed to send bulk messages. Errors: {Errors}", sendResult.Errors);
        }
        else
        {
            logger.LogInformation("Successfully sent bulk messages");
        }
        
        return sendResult;
    }
}