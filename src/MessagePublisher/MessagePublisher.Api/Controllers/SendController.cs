using CoreLib.Common;
using MessagePublisher.Api.Controllers.Base;
using MessagePublisher.Api.Extensions;
using MessagePublisher.Logic.Interfaces.Managers;
using MessagePublisher.Logic.Models.Requests.Send;
using Microsoft.AspNetCore.Mvc;

namespace MessagePublisher.Api.Controllers;

public class SendController(ISendManager sendManager) : ApiControllerBase
{
    [HttpPost(Routes.Routes.SendMessage)]
    [ProducesDefaultResponseType(typeof(OperationResult))]
    [ProducesResponseType<OperationResult>(StatusCodes.Status200OK)]
    [ProducesResponseType<OperationResult>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> SendMessage(SendMessageRequest sendMessage)
    {
        var sendResult = await sendManager.Send(sendMessage);
        return sendResult.ToActionResult();
    }
}