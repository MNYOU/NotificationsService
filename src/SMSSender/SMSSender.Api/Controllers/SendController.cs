using CoreLib.Common;
using Microsoft.AspNetCore.Mvc;
using SMSSender.Api.Controllers.Base;
using SMSSender.SendLogic.Interfaces.Managers;
using SMSSender.SendLogic.Models.DTO.SendModels;
using SMSSender.SendLogic.Models.Requests.Send;
using SMSSender.Api.Extensions;

namespace SMSSender.Api.Controllers
{
    public class SendController(ISendManager sendManager) : ApiControllerBase
    {
        [HttpPost(Routes.Routes.SendMessage)]
        [ProducesDefaultResponseType(typeof(OperationResult<SendMessage>))]
        [ProducesResponseType<OperationResult<SendMessage>>(StatusCodes.Status200OK)]
        [ProducesResponseType<OperationResult<SendMessage>>(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SendMessage>> SendMessage(SendMessageRequest sendMessage)
        {
            var sendResult = await sendManager.SendMessage(sendMessage);
            return sendResult.ToActionResult();
        }

        [HttpPost(Routes.Routes.SendMessages)]
        [ProducesDefaultResponseType(typeof(BatchOperationResult<SendMessage>))]
        [ProducesResponseType<BatchOperationResult<SendMessage>>(StatusCodes.Status200OK)]
        [ProducesResponseType<BatchOperationResult<SendMessage>>(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SendMessage>> SendMessages(IEnumerable<SendMessageRequest> sendMessages)
        {
            var sendResult = await sendManager.SendBulk(sendMessages);
            return sendResult.ToActionResult();
        }
    }
}
