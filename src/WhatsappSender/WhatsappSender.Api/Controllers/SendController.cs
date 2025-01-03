using Contracts.WhatsApp.Requests;
using CoreLib.Common;
using Microsoft.AspNetCore.Mvc;
using WhatsappSender.Api.Controllers.Base;
using WhatsappSender.Api.Extensions;
using WhatsappSender.SendLogic.Interfaces.Managers;
using WhatsappSender.SendLogic.Models.DTO.SendModels;

namespace WhatsappSender.Api.Controllers;

public class SendController(ISendManager sendManager) : ApiControllerBase
{
    [HttpPost(EmailSender.Api.Routes.Routes.SendMessages)]
    [ProducesDefaultResponseType(typeof(BatchOperationResult<SendMessage>))]
    [ProducesResponseType<BatchOperationResult<SendMessage>>(StatusCodes.Status200OK)]
    [ProducesResponseType<BatchOperationResult<SendMessage>>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SendMessage>> SendMessages(ICollection<WhatsAppMessageRequest> sendMessages)
    {
        var sendResult = await sendManager.SendBulk(sendMessages);
        return sendResult.ToActionResult();
    }

    [HttpPost(EmailSender.Api.Routes.Routes.SendMessage)]
    [ProducesDefaultResponseType(typeof(OperationResult<SendMessage>))]
    [ProducesResponseType<OperationResult<SendMessage>>(StatusCodes.Status200OK)]
    [ProducesResponseType<OperationResult<SendMessage>>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SendMessage>> SendMessage(WhatsAppMessageRequest sendMessage)
    {
        var sendResult = await sendManager.SendMessage(sendMessage);
        return sendResult.ToActionResult();
    }
}