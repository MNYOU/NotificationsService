using Contracts.WhatsApp.Requests;
using CoreLib.Common;
using MessagePublisher.Logic.Interfaces.Managers;
using MessagePublisher.Logic.Interfaces.Services;
using MessagePublisher.Logic.Mappers;
using MessagePublisher.Logic.Models.DTO.SendModels;

namespace MessagePublisher.Logic.Managers;

public class WhatsAppPublisherManager(IPublisherService<WhatsAppMessageRequest> service)  : IPublisherManager
{
    public async Task<OperationResult> Send(SendMessage sendMessage)
    {
        var message = sendMessage.ToWhatsAppMessageRequest();
        return await service.Publish(message);
    }
}