using CoreLib.Common;
using Microsoft.Extensions.Logging;
using WhatsappBusiness.CloudApi.Messages.Requests;
using WhatsappSender.SendLogic.Interfaces.Services;
using WhatsappSender.SendLogic.Models.DTO.SendModels;

namespace WhatsappSender.SendLogic.Services;

internal sealed class ModelsService(ILogger<ModelsService> logger) : IModelsService
{
    public OperationResult<TextMessageRequest> CreateTextMessageRequest(
        string phoneNumber, string text)
    {
        var textMessageRequest = new TextMessageRequest
        {
            To = phoneNumber,
            Text = new WhatsAppText
            {
                Body = text,
                PreviewUrl = false
            }
        };
        logger.LogDebug("TextMessageRequest created for phone number: {PhoneNumber}", phoneNumber);
        return textMessageRequest;
    }

    public OperationResult<DocumentMessageByUrlRequest> CreateFileMessageRequest(
        string phoneNumber, SendAttachment pathAttachment)
    {
        var documentMessage = new DocumentMessageByUrlRequest
        {
            To = phoneNumber,
            Document = new MediaDocumentUrl
            {
                Link = pathAttachment.FileUrl
            }
        };
        logger.LogDebug("DocumentMessageByUrlRequest created for phone number: {PhoneNumber}, File: {FileName}", 
            phoneNumber, pathAttachment.FileName);
        return documentMessage;
    }
}