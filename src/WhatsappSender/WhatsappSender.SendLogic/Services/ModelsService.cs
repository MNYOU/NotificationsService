using WhatsappBusiness.CloudApi.Messages.Requests;
using WhatsappSender.Core.Common;
using WhatsappSender.SendLogic.Interfaces.Services;
using WhatsappSender.SendLogic.Models.DTO.SendModels;

namespace WhatsappSender.SendLogic.Services;

internal sealed class ModelsService : IModelsService
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
        return documentMessage;
    }
}