using CoreLib.Common;
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
    
    public async Task<OperationResult<SendMessage>> SendMessage(SendMessageRequest message)
    {
        logger.LogInformation("Received send message request for recipient: {Recipient}", message.Recipient);
        var sendMessage = message.ToApplicationMessage();
        logger.LogDebug("Mapped SendMessageRequest to SendMessage for recipient: {Recipient}", message.Recipient);
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
}