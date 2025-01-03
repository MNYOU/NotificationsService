using Contracts.Email.Requests;
using CoreLib.Common;
using EmailSender.Api.Controllers.Base;
using EmailSender.Api.Extensions;
using EmailSender.SendLogic.Interfaces.Managers;
using EmailSender.SendLogic.Models.DTO.SendModels;
using Microsoft.AspNetCore.Mvc;

namespace EmailSender.Api.Controllers;

public class SendController(ISendManager sendManager) : ApiControllerBase
{
    [HttpPost(Routes.Routes.SendMessages)]
    [ProducesDefaultResponseType(typeof(BatchOperationResult<SendMessage>))]
    [ProducesResponseType<BatchOperationResult<SendMessage>>(StatusCodes.Status200OK)]
    [ProducesResponseType<BatchOperationResult<SendMessage>>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SendMessage>> SendMessages(ICollection<EmailMessageRequest> sendMessages)
    {
        var sendResult = await sendManager.SendBulk(sendMessages);
        return sendResult.ToActionResult();
    }

    [HttpPost(Routes.Routes.SendMessage)]
    [ProducesDefaultResponseType(typeof(OperationResult<SendMessage>))]
    [ProducesResponseType<OperationResult<SendMessage>>(StatusCodes.Status200OK)]
    [ProducesResponseType<OperationResult<SendMessage>>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SendMessage>> SendMessage(EmailMessageRequest sendMessage)
    {
        var sendResult = await sendManager.SendMessage(sendMessage);
        return sendResult.ToActionResult();
    }
}