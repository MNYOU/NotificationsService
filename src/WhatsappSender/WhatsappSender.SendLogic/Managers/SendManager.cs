﻿using Contracts.WhatsApp.Requests;
using CoreLib.Common;
using CoreLib.Logging.Extensions;
using Microsoft.Extensions.Logging;
using WhatsappSender.SendLogic.Extensions;
using WhatsappSender.SendLogic.Interfaces.Managers;
using WhatsappSender.SendLogic.Interfaces.Services;
using WhatsappSender.SendLogic.Models.DTO.SendModels;

namespace WhatsappSender.SendLogic.Managers;

public class SendManager(ISendService sendService, ILogger<SendManager> logger) : ISendManager
{
    public async Task<BatchOperationResult<SendMessage>> SendBulk(ICollection<WhatsAppMessageRequest> messages)
    {
        logger.LogInformation("Received bulk send messages request. Message count: {Count}", messages.Count);
        var sendMessages = messages.ToApplicationMessages();
        logger.LogDebug("Mapped SendMessageRequest to SendMessage. Message count: {Count}", sendMessages.Count);
        var sendResult = await sendService.SendBulk(sendMessages);
        logger.LogBatchResult(sendResult);
        return sendResult;
    }

    public async Task<OperationResult<SendMessage>> SendMessage(WhatsAppMessageRequest message)
    {
        logger.LogInformation("Received send message request for recipient: {Recipient}", message.SendRecipient.PhoneNumber);
        var sendMessage = message.ToApplicationMessage();
        logger.LogDebug("Mapped SendMessageRequest to SendMessage for recipient: {Recipient}",  sendMessage.Recipient);
        var sendResult = await sendService.Send(sendMessage);
        logger.LogResult(sendResult);
        return sendResult;
    }
}