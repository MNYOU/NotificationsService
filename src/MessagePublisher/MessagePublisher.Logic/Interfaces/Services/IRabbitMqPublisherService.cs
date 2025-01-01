using MessagePublisher.Core.Common;
using MessagePublisher.Logic.Models.DTO.SendModels;

namespace MessagePublisher.Logic.Interfaces.Services;

public interface IPublisherService
{
    public Task<OperationResult> Publish(SendMessage message);
}