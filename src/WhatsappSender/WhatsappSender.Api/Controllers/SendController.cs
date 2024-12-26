﻿using Microsoft.AspNetCore.Mvc;
using WhatsappSender.Api.Controllers.Base;
using WhatsappSender.Api.Extensions;
using WhatsappSender.Core.Common;
using WhatsappSender.SendLogic.Interfaces.Managers;
using WhatsappSender.SendLogic.Models.DTO.SendModels;
using WhatsappSender.SendLogic.Models.Requests.Send;

namespace WhatsappSender.Api.Controllers;

public class SendController(ISendManager sendManager) : ApiControllerBase
{
    [HttpPost(EmailSender.Api.Routes.Routes.SendMessages)]
    [ProducesDefaultResponseType(typeof(BatchOperationResult<SendMessage>))]
    [ProducesResponseType<BatchOperationResult<SendMessage>>(StatusCodes.Status200OK)]
    [ProducesResponseType<BatchOperationResult<SendMessage>>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SendMessage>> SendMessages(IEnumerable<SendMessageRequest> sendMessages)
    {
        var sendResult = await sendManager.SendBulk(sendMessages);
        return sendResult.ToActionResult();
    }

    [HttpPost(EmailSender.Api.Routes.Routes.SendMessage)]
    [ProducesDefaultResponseType(typeof(OperationResult<SendMessage>))]
    [ProducesResponseType<OperationResult<SendMessage>>(StatusCodes.Status200OK)]
    [ProducesResponseType<OperationResult<SendMessage>>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SendMessage>> SendMessage(SendMessageRequest sendMessage)
    {
        var sendResult = await sendManager.SendMessage(sendMessage);
        return sendResult.ToActionResult();
    }
}