using WhatsappBusiness.CloudApi.Exceptions;
using WhatsappBusiness.CloudApi.Interfaces;
using WhatsappBusiness.CloudApi.Messages.Requests;
using WhatsappSender.Core.Common;
using WhatsappSender.SendLogic.Interfaces.Services;
using WhatsappSender.SendLogic.Models.DTO.SendModels;

namespace WhatsappSender.SendLogic.Services;

public sealed class SendService(IModelsService modelsService, IWhatsAppBusinessClient whatsAppBusinessClient)
    : ISendService
{
    public async Task<OperationResult<SendMessage>> Send(SendMessage message)
    {
        var sendResult = await SendMessage(message);
        return sendResult;
    }

    public async Task<BatchOperationResult<SendMessage>> SendBulk(IEnumerable<SendMessage> messages)
    {
        var sendResults = new List<OperationResult<SendMessage>>();
        foreach (var message in messages)
        {
            var result = await SendMessage(message);
            sendResults.Add(result);
        }

        return BatchOperationResult<SendMessage>.FromOperationResults(sendResults);
    }

    private async Task<OperationResult<SendMessage>> SendMessage(SendMessage message)
    {
        var sendTextResult = await SendTextMessage(message.Recipient, message.Content);
        if (sendTextResult.IsFail)
        {
            return sendTextResult.Error!;
        }

        var sendFilesTasks = message.Attachments.Select(a => SendFileMessage(message.Recipient, a));
        var sendFilesResults = await Task.WhenAll(sendFilesTasks);
        var bathSendResult = BatchOperationResult.FromOperationResults(sendFilesResults);
        if (bathSendResult.IsFailure)
        {
            return bathSendResult.Errors.First();
        }

        return message;
    }

    private async Task<OperationResult> SendTextMessage(string recipient, string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return OperationResult.Success();
        }

        var textTemplateMessageResult =
            modelsService.CreateTextMessageRequest(recipient, content);
        if (textTemplateMessageResult.IsFail)
        {
            return textTemplateMessageResult.Error!;
        }

        var sendResult = await SendTextMessage(textTemplateMessageResult.Result!);
        return sendResult;
    }

    private async Task<OperationResult> SendFileMessage(string recipient, SendAttachment attachment)
    {
        var textTemplateMessageResult =
            modelsService.CreateFileMessageRequest(recipient, attachment);
        if (textTemplateMessageResult.IsFail)
        {
            return textTemplateMessageResult.Error!;
        }

        var sendResult = await SendFileMessage(textTemplateMessageResult.Result!);
        return sendResult;
    }

    private async Task<OperationResult> SendTextMessage(TextMessageRequest textTemplateMessage)
    {
        try
        {
            await whatsAppBusinessClient.SendTextMessageAsync(textTemplateMessage);
        }
        catch (WhatsappBusinessCloudAPIException ex)
        {
            return Error.BadRequest($"Не удалось отправить сообщение получателю. " +
                                    $"Ошибка с whatsapp отправлением с ошибкой WhatsappBusinessCloudAPIException. {ex}");
        }
        catch (Exception ex)
        {
            return Error.BadRequest($"Не удалось отправить сообщение получателю. Ошибка с whatsapp отправлением. {ex}");
        }

        return OperationResult.Success();
    }

    private async Task<OperationResult> SendFileMessage(DocumentMessageByUrlRequest fileTemplateMessage)
    {
        try
        {
            await whatsAppBusinessClient.SendDocumentAttachmentMessageByUrlAsync(fileTemplateMessage);
        }
        catch (WhatsappBusinessCloudAPIException ex)
        {
            return Error.BadRequest($"Не удалось отправить сообщение получателю. " +
                                    $"Ошибка с whatsapp отправлением с типом WhatsappBusinessCloudAPIException. {ex}");
        }
        catch (Exception ex)
        {
            return Error.BadRequest($"Не удалось отправить сообщение получателю. Ошибка с whatsapp отправлением. {ex}");
        }

        return OperationResult.Success();
    }
}