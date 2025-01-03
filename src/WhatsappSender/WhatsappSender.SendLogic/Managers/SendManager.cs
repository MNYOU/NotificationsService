using CoreLib.Common;
using CoreLib.Logging.Extensions;
using Microsoft.Extensions.Logging;
using WhatsappSender.SendLogic.Extensions;
using WhatsappSender.SendLogic.Interfaces.Managers;
using WhatsappSender.SendLogic.Interfaces.Services;
using WhatsappSender.SendLogic.Models.DTO.SendModels;
using WhatsappSender.SendLogic.Models.Requests.Send;

namespace WhatsappSender.SendLogic.Managers;

public class SendManager(ISendService sendService, ILogger<SendManager> logger) : ISendManager
{
    public async Task<BatchOperationResult<SendMessage>> SendBulk(ICollection<SendMessageRequest> messages)
    {
        logger.LogInformation("Received bulk send messages request. Message count: {Count}", messages.Count);
        var sendMessages = messages.ToApplicationMessages();
        logger.LogDebug("Mapped SendMessageRequest to SendMessage. Message count: {Count}", sendMessages.Count);
        var sendResult = await sendService.SendBulk(sendMessages);

        if (sendResult.IsSuccess)
        {
            logger.LogInformation("Successfully sent bulk messages");
        }
        else
        {
            logger.LogError("Failed to send bulk messages. Errors: {Errors}", sendResult.Errors);
        }

        return sendResult;
    }

    public async Task<OperationResult<SendMessage>> SendMessage(SendMessageRequest message)
    {
        logger.LogInformation("Received send message request for recipient: {Recipient}", message.Recipient);
        var sendMessage = message.ToApplicationMessage();
        logger.LogDebug("Mapped SendMessageRequest to SendMessage for recipient: {Recipient}", message.Recipient);
        var sendResult = await sendService.Send(sendMessage);

        if (sendResult.IsSuccess)
        {

            logger.LogInformation("Successfully sent message to recipient: {Recipient}", message.Recipient);
        }
        else
        {
            logger.LogError("Failed to send message to recipient: {Recipient}. Error: {Error}", message.Recipient, sendResult.Error);
        }

        return sendResult;
    }
}