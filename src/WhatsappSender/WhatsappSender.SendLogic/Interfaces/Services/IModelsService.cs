using CoreLib.Common;
using WhatsappBusiness.CloudApi.Messages.Requests;
using WhatsappSender.SendLogic.Models.DTO.SendModels;

namespace WhatsappSender.SendLogic.Interfaces.Services;

public interface IModelsService
{
    public OperationResult<TextMessageRequest> CreateTextMessageRequest(string phoneNumber, string text);

    public OperationResult<DocumentMessageByUrlRequest> CreateFileMessageRequest(string phoneNumber,
        SendAttachment pathAttachment);
}