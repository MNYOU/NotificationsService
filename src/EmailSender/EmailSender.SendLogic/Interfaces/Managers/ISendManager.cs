using Contracts.Email.Requests;
using CoreLib.Common;
using EmailSender.SendLogic.Models.DTO.SendModels;

namespace EmailSender.SendLogic.Interfaces.Managers;

public interface ISendManager
{
    public Task<BatchOperationResult<SendMessage>> SendBulk(ICollection<EmailMessageRequest> messages);
    public Task<OperationResult<SendMessage>> SendMessage(EmailMessageRequest message);
}