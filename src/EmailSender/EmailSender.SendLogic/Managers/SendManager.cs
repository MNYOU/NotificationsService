using CoreLib.Common;
using EmailSender.SendLogic.Extensions;
using EmailSender.SendLogic.Interfaces.Managers;
using EmailSender.SendLogic.Interfaces.Services;
using EmailSender.SendLogic.Models.DTO.SendModels;
using EmailSender.SendLogic.Models.Requests.Send;

namespace EmailSender.SendLogic.Managers;

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