using WhatsappSender.Core.Common;
using WhatsappSender.SendLogic.Extensions;
using WhatsappSender.SendLogic.Interfaces.Managers;
using WhatsappSender.SendLogic.Interfaces.Services;
using WhatsappSender.SendLogic.Models.DTO.SendModels;
using WhatsappSender.SendLogic.Models.Requests.Send;

namespace WhatsappSender.SendLogic.Managers;

public class SendManager(ISendService sendService) : ISendManager
{
    public async Task<BatchOperationResult<SendMessage>> SendBulk(IEnumerable<SendMessageRequest> messages)
    {
        var sendMessages = messages.ToApplicationMessages();
        var sendResult = await sendService.SendBulk(sendMessages);
        return sendResult;
    }

    public async Task<OperationResult<SendMessage>> SendMessage(SendMessageRequest message)
    {
        var sendMessage = message.ToApplicationMessage();
        var sendResult = await sendService.Send(sendMessage);
        return sendResult;
    }
}