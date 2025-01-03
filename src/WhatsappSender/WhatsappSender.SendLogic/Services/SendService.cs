using CoreLib.Common;
using CoreLib.Logging.Extensions;
using Microsoft.Extensions.Logging;
using WhatsappBusiness.CloudApi.Exceptions;
using WhatsappBusiness.CloudApi.Interfaces;
using WhatsappBusiness.CloudApi.Messages.Requests;
using WhatsappSender.SendLogic.Interfaces.Services;
using WhatsappSender.SendLogic.Models.DTO.SendModels;

namespace WhatsappSender.SendLogic.Services;

public sealed class SendService(IModelsService modelsService, IWhatsAppBusinessClient whatsAppBusinessClient,
    ILogger<SendService> logger)
    : ISendService
{
    public async Task<OperationResult<SendMessage>> Send(SendMessage message)
    {
        logger.LogInformation("Sending message to: {Recipient}", message.Recipient);
        var sendResult = await SendMessage(message);
        if (sendResult.IsSuccess)
        {
            logger.LogInformation("Successfully sent message to: {Recipient}", message.Recipient);
        }
        else
        {
            logger.LogError("Failed to send message to: {Recipient}. Error: {Error}", message.Recipient, sendResult.Error);
        }
        return sendResult;
    }

    public async Task<BatchOperationResult<SendMessage>> SendBulk(IReadOnlyCollection<SendMessage> messages)
    {
        logger.LogInformation("Sending bulk messages. Total messages: {Count}", messages.Count);
        var sendResults = new List<OperationResult<SendMessage>>();
        foreach (var message in messages)
        {
            var result = await SendMessage(message);
            sendResults.Add(result);
        }
        var batchResult = BatchOperationResult<SendMessage>.FromOperationResults(sendResults);
        logger.LogBatchResult(batchResult);
        return batchResult;
    }

    private async Task<OperationResult<SendMessage>> SendMessage(SendMessage message)
    {
        logger.LogDebug("Starting to send message with text content and attachments to: {Recipient}", message.Recipient);
        var sendTextResult = await SendTextMessage(message.Recipient, message.Content);
        if (sendTextResult.IsFail)
        {
            logger.LogError("Failed to send text message to: {Recipient}. Error: {Error}", message.Recipient, sendTextResult.Error);
            return sendTextResult.Error!;
        }

        var sendFilesTasks = message.Attachments.Select(a => SendFileMessage(message.Recipient, a));
        var sendFilesResults = await Task.WhenAll(sendFilesTasks);
        var bathSendResult = BatchOperationResult.FromOperationResults(sendFilesResults);
        if (bathSendResult.IsFailure)
        {
            logger.LogError("Failed to send attachments for message to: {Recipient}. Errors: {Errors}", message.Recipient, bathSendResult.Errors);
            return bathSendResult.Errors.First();
        }
        logger.LogDebug("Successfully sent message with text content and attachments to: {Recipient}", message.Recipient);
        return message;
    }

    private async Task<OperationResult> SendTextMessage(string recipient, string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            logger.LogDebug("No text content to send for: {Recipient}", recipient);
            return OperationResult.Success();
        }

        logger.LogDebug("Creating text message request for: {Recipient}", recipient);
        var textTemplateMessageResult = modelsService.CreateTextMessageRequest(recipient, content);
        if (textTemplateMessageResult.IsFail)
        {
            logger.LogError("Failed to create text message request for: {Recipient}. Error: {Error}", 
                recipient, textTemplateMessageResult.Error);
            return textTemplateMessageResult.Error!;
        }

        var sendResult = await SendTextMessage(textTemplateMessageResult.Result!);
        logger.LogResult(sendResult);
        return sendResult;
    }

    private async Task<OperationResult> SendFileMessage(string recipient, SendAttachment attachment)
    {
        logger.LogDebug("Creating file message request for: {Recipient}, File: {FileName}", 
            recipient, attachment.FileName);
        var textTemplateMessageResult = modelsService.CreateFileMessageRequest(recipient, attachment);
        if (textTemplateMessageResult.IsFail)
        {
            logger.LogError("Failed to create file message request for: {Recipient}, File: {FileName}. Error: {Error}", 
                recipient, attachment.FileName, textTemplateMessageResult.Error);
            return textTemplateMessageResult.Error!;
        }

        logger.LogDebug("Sending file message for: {Recipient}, File: {FileName}", recipient, attachment.FileName);
        var sendResult = await SendFileMessage(textTemplateMessageResult.Result!);
        logger.LogResult(sendResult);
        return sendResult;
    }

    private async Task<OperationResult> SendTextMessage(TextMessageRequest textTemplateMessage)
    {
        try
        {
            logger.LogDebug("Sending text message via WhatsApp API to: {Recipient}", textTemplateMessage.To);
            await whatsAppBusinessClient.SendTextMessageAsync(textTemplateMessage);
            logger.LogInformation("Successfully sent text message via WhatsApp API to: {Recipient}", 
                textTemplateMessage.To);
        }
        catch (WhatsappBusinessCloudAPIException ex)
        {
            logger.LogError("Failed to send text message via WhatsApp API to: {Recipient}. Error: {Error}", 
                textTemplateMessage.To, ex);
            return Error.BadRequest($"Не удалось отправить сообщение получателю. " +
                                    $"Ошибка с whatsapp отправлением с ошибкой WhatsappBusinessCloudAPIException. {ex}");
        }
        catch (Exception ex)
        {
            logger.LogError("Failed to send text message via WhatsApp API to: {Recipient}. Error: {Error}", 
                textTemplateMessage.To, ex);
            return Error.BadRequest($"Не удалось отправить сообщение получателю. Ошибка с whatsapp отправлением. {ex}");
        }

        return OperationResult.Success();
    }

    private async Task<OperationResult> SendFileMessage(DocumentMessageByUrlRequest fileTemplateMessage)
    {
        try
        {
            logger.LogDebug("Sending file message via WhatsApp API to: {Recipient}, File: {FileUrl}", 
                fileTemplateMessage.To, fileTemplateMessage.Document?.Link);
            await whatsAppBusinessClient.SendDocumentAttachmentMessageByUrlAsync(fileTemplateMessage);
        }
        catch (WhatsappBusinessCloudAPIException ex)
        {
            logger.LogInformation("Successfully sent file message via WhatsApp API to: {Recipient}, File: {FileUrl}", 
                fileTemplateMessage.To, fileTemplateMessage.Document?.Link);
            return Error.BadRequest($"Не удалось отправить сообщение получателю. " +
                                    $"Ошибка с whatsapp отправлением с типом WhatsappBusinessCloudAPIException. {ex}");
        }
        catch (Exception ex)
        {
            logger.LogError("Failed to send file message via WhatsApp API to: {Recipient}, File: {FileUrl}. Error: {Error}", 
                fileTemplateMessage.To, fileTemplateMessage.Document?.Link, ex);
            return Error.BadRequest($"Не удалось отправить сообщение получателю. Ошибка с whatsapp отправлением. {ex}");
        }

        return OperationResult.Success();
    }
}