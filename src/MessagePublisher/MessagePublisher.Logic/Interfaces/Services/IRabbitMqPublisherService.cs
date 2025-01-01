using MessagePublisher.Logic.Models.DTO.SendModels;
using WhatsappSender.Core.Common;

namespace MessagePublisher.Logic.Interfaces.Services;

public interface IPublisherService
{
    public Task<OperationResult> Publish(SendMessage message);
}